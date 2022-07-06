using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Pyto.Controllers;

public abstract class ApplicationControllerBase : ControllerBase
{
	public string UserEmail => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
