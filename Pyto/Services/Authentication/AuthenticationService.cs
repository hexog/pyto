using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pyto.Data.Users;
using Pyto.Models.Extensions;

namespace Pyto.Services.Authentication;

public record AuthenticationResult((string Token, DateTime ValidTo) AccessToken,
								   (string Token, DateTime ValidTo) RefreshToken);

public interface IAuthenticationService
{
	Task<AuthenticationResult> GetUserTokensAsync(UserDbo user);
	Task<AuthenticationResult?> RefreshTokens(string refreshToken);
}

public class AuthenticationService : IAuthenticationService
{
	private readonly UserManager<UserDbo> userManager;
	private readonly IConfiguration configuration;
	private readonly IRefreshTokenRepository refreshTokenRepository;
	private readonly ILogger<AuthenticationService> logger;
	private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

	public AuthenticationService(UserManager<UserDbo> userManager, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, ILogger<AuthenticationService> logger)
	{
		this.userManager = userManager;
		this.configuration = configuration;
		this.refreshTokenRepository = refreshTokenRepository;
		this.logger = logger;
	}

	public Task<AuthenticationResult> GetUserTokensAsync(UserDbo user) => GetUserTokensAsync(user, null);

	public async Task<AuthenticationResult> GetUserTokensAsync(UserDbo user, RefreshTokenDbo? previousRefreshToken)
	{
		var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

		var claims = new List<Claim>(userRoles.Count + 4)
		{
			new(JwtRegisteredClaimNames.Sub, user.Email),
			new(JwtRegisteredClaimNames.Jti, Rng.GetString(32)),
		};

		claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

		var accessToken = GenerateAccessToken(claims);
		var accessTokenStr = new JwtSecurityTokenHandler().WriteToken(accessToken);
		var accessTokenExpiration = accessToken.ValidTo;

		var refreshToken = GenerateRefreshToken(previousRefreshToken);
		await refreshTokenRepository.AddAsync(refreshToken.Token, user.Id, refreshToken.ValidTo).ConfigureAwait(false);

		return new AuthenticationResult((accessTokenStr, accessTokenExpiration),
										refreshToken);
	}

	public async Task<AuthenticationResult?> RefreshTokens(string refreshToken)
	{
		var existingToken = await refreshTokenRepository.FindValidAsync(refreshToken).ConfigureAwait(false);
		if (existingToken is null)
		{
			return null;
		}

		await refreshTokenRepository.RemoveAsync(existingToken).ConfigureAwait(false);

		return await GetUserTokensAsync(existingToken.User, existingToken).ConfigureAwait(false);
	}

	private (string Token, DateTime ValidTo) GenerateRefreshToken(RefreshTokenDbo? previousToken = null)
	{
		var token = Rng.GetString(32);

		DateTime validTo;

		if (previousToken is not null)
		{
			validTo = previousToken.ValidTo;
		}
		else
		{
			var refreshTokenDuration = configuration.GetValue<double?>("Authentication:JWT:RefreshTokenDurationInHours");

			if (refreshTokenDuration is null)
			{
				logger.LogWarning("Refresh token duration is not set in configuration, using default value");
				refreshTokenDuration = 4;
			}

			validTo = DateTime.UtcNow.AddHours(refreshTokenDuration.Value);
		}

		return (token, validTo);
	}

	private JwtSecurityToken GenerateAccessToken(IList<Claim> claims)
	{
		var authSigningKey =
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:JWT:Key"]));

		var accessTokenDuration = configuration.GetValue<double?>("Authentication:JWT:AccessTokenDurationInMinutes");
		if (accessTokenDuration is null)
		{
			logger.LogWarning("Access token duration is not set in configuration, using default value");
			accessTokenDuration = 5;
		}

		return new JwtSecurityToken(
			issuer: configuration["Authentication:JWT:ValidIssuer"],
			audience: configuration["Authentication:JWT:ValidAudience"],
			expires: DateTime.UtcNow.AddMinutes(accessTokenDuration.Value),
			claims: claims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
		);
	}
}
