using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Pyto.Data.Users;

[Index(nameof(RefreshToken))]
[Index(nameof(ValidTo))]
public class RefreshTokenDbo
{
	[Key] public Guid Id { get; set; }

	public UserDbo User { get; set; }
	[Required] public Guid UserId { get; set; }

	[Required] public string RefreshToken { get; set; }
	[Required] public DateTime Created { get; set; }
	[Required] public DateTime ValidTo { get; set; }
}
