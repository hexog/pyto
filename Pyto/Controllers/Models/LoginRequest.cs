#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pyto.Controllers.Models;

public class LoginRequest
{
	[Required(ErrorMessage = "Email is required")]
	[EmailAddress]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	[PasswordPropertyText]
	public string Password { get; set; }
}
