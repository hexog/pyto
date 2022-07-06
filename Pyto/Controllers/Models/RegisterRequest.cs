#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pyto.Controllers.Models;

public class RegisterRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[PasswordPropertyText]
	public string Password { get; set; }
}
