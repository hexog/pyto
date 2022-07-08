using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Pyto.Controllers;

public abstract class ApplicationControllerBase : ControllerBase
{
	protected string UserEmail => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
