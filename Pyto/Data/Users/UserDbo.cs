#nullable disable
using Microsoft.AspNetCore.Identity;

namespace Pyto.Data.Users;

public class UserDbo : IdentityUser<Guid>
{
	public string RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiration { get; set; }
}
