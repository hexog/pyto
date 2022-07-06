using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pyto.Controllers.Authorization;
using Pyto.Controllers.Authorization.Helpers;
using Pyto.Controllers.Models;
using Pyto.Data.Users;
using Pyto.Models.Extensions;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Pyto.Controllers;

[Route("account")]
public class AccountController : ApplicationControllerBase
{
	private readonly UserManager<UserDbo> userManager;
	private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
	private readonly IConfiguration configuration;
	private readonly RoleManager<IdentityRole<Guid>> roleManager;

	public AccountController(UserManager<UserDbo> userManager, IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager)
	{
		this.userManager = userManager;
		this.configuration = configuration;
		this.roleManager = roleManager;
	}

	[HttpPost("register")]
	[Produces(ContentTypes.ApplicationJson)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
	{
		if (this.ModelState.IsValid == false)
		{
			return this.BadRequest(this.ModelState);
		}

		var email = await userManager.FindByEmailAsync(request.Email);
		if (email is not null)
		{
			return this.BadRequest(new ErrorResponse("User already exists"));
		}

		var user = new UserDbo
		{
			Email = request.Email,
			UserName = request.Email,
		};

		var identityResult = await userManager.CreateAsync(user, request.Password);
		if (identityResult.Succeeded == false)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError,
								   new ErrorResponse("User creation failed! Please check user details and try again."));
		}

		await userManager.AddToRoleAsync(user, Roles.User);

		return await LoginInnerAsync(user);
	}

	[Route("login")]
	[HttpPost]
	public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
	{
		var user = await userManager.FindByEmailAsync(loginRequest.Email);
		if (user is not null && await userManager.CheckPasswordAsync(user, loginRequest.Password))
		{
			return await LoginInnerAsync(user);
		}

		return this.Unauthorized(new ErrorResponse("Invalid email or password."));
	}

	private async Task<LoginResponse> LoginInnerAsync(UserDbo user)
	{
		var userRoles = await userManager.GetRolesAsync(user);

		var claims = new List<Claim>(userRoles.Count + 4)
		{
			new(JwtRegisteredClaimNames.Sub, user.Email),
			new(JwtRegisteredClaimNames.Jti, rng.GetString(32)),
		};

		claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

		var token = GenerateUserToken(claims);

		return new LoginResponse
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token),
			Expiration = token.ValidTo,
		};
	}

	private JwtSecurityToken GenerateUserToken(IList<Claim> claims)
	{
		var authSigningKey =
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

		return new JwtSecurityToken(
			issuer: configuration["JWT:ValidIssuer"],
			audience: configuration["JWT:ValidAudience"],
			expires: DateTime.Now.AddHours(8),
			claims: claims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
		);
	}
}
