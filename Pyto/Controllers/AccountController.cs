using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pyto.Controllers.Authorization;
using Pyto.Controllers.Helpers;
using Pyto.Controllers.Models;
using Pyto.Data.Users;
using Pyto.Services;
using Pyto.Services.Authentication;

namespace Pyto.Controllers;

[Route("account")]
public class AccountController : ApplicationControllerBase
{
	private readonly UserManager<UserDbo> userManager;
	private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
	private readonly IConfiguration configuration;
	private readonly RoleManager<IdentityRole<Guid>> roleManager;
	private readonly IAuthenticationService authenticationService;

	public AccountController(UserManager<UserDbo> userManager, IConfiguration configuration,
							 RoleManager<IdentityRole<Guid>> roleManager, IAuthenticationService authenticationService)
	{
		this.userManager = userManager;
		this.configuration = configuration;
		this.roleManager = roleManager;
		this.authenticationService = authenticationService;
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

		var email = await userManager.FindByEmailAsync(request.Email).ConfigureAwait(true);
		if (email is not null)
		{
			return this.BadRequest(new ErrorResponse("User already exists"));
		}

		var user = new UserDbo
		{
			Email = request.Email,
			UserName = request.Email,
		};

		var identityResult = await userManager.CreateAsync(user, request.Password).ConfigureAwait(true);
		if (identityResult.Succeeded == false)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError,
								   new ErrorResponse("User creation failed! Please check user details and try again."));
		}

		await userManager.AddToRoleAsync(user, Roles.User).ConfigureAwait(true);

		return await LoginInnerAsync(user).ConfigureAwait(true);
	}

	[Route("login")]
	[HttpPost]
	public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
	{
		var user = await userManager.FindByEmailAsync(loginRequest.Email).ConfigureAwait(true);
		if (user is not null && await userManager.CheckPasswordAsync(user, loginRequest.Password).ConfigureAwait(true))
		{
			return await LoginInnerAsync(user).ConfigureAwait(true);
		}

		return this.Unauthorized(new ErrorResponse("Invalid email or password."));
	}

	[HttpPost("refresh")]
	public async Task<ActionResult<LoginResponse>> Refresh(string refreshToken)
	{
		var tokens = await authenticationService.RefreshTokens(refreshToken).ConfigureAwait(true);
		if (tokens is null)
		{
			return this.NotFound(new ErrorResponse("Refresh token not found"));
		}

		return new LoginResponse
		{
			AccessToken = tokens.AccessToken.Token,
			RefreshToken = tokens.RefreshToken.Token,
			AccessTokenValidTo = tokens.AccessToken.ValidTo,
		};
	}

	private async Task<LoginResponse> LoginInnerAsync(UserDbo user)
	{
		var authenticationResult = await authenticationService.GetUserTokensAsync(user).ConfigureAwait(true);

		return new LoginResponse
		{
			AccessToken = authenticationResult.AccessToken.Token,
			AccessTokenValidTo = authenticationResult.AccessToken.ValidTo,
			RefreshToken = authenticationResult.RefreshToken.Token,
		};
	}
}
