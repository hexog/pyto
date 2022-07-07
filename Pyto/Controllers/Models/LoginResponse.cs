#nullable disable
namespace Pyto.Controllers.Models;

public class LoginResponse
{
	public string AccessToken { get; set; }
	public DateTime AccessTokenValidTo { get; set; }
	public string RefreshToken { get; set; }
}
