using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Pyto.Controllers.Helpers;
using Pyto.Controllers.Models;
using Pyto.Data.Users;
using Pyto.Models;

namespace Pyto.Controllers;

[Route("todo-list")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TodoListController : ApplicationControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Produces(ContentTypes.ApplicationJson)]
	public ActionResult<TodoListResponse> GetTodos()
	{
		return new TodoListResponse
		{
			TodoList = new TodoResponse[]
			{
				new TodoResponse
				{
					Content = "Foo content",
					Id = Guid.Parse("1B7DD6C4-5181-43B0-B20F-A9D1C96116A5"),
					TodoState = TodoState.Checked,
				}
			}
		};
	}

	public Task<ActionResult<TodoResponse>> CreateTodo()
	{
		throw new NotImplementedException();
	}
}
