using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pyto.Controllers.Helpers;
using Pyto.Controllers.Models;
using Pyto.Data.Users;
using Pyto.Services.TodoList;

namespace Pyto.Controllers;

[Route("api/v0/todo-list")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TodoListController : ApplicationControllerBase
{
	private readonly ITodoListServiceFactory todoListServiceFactory;
	private readonly UserManager<UserDbo> userManager;

	public TodoListController(ITodoListServiceFactory todoListServiceFactory, UserManager<UserDbo> userManager)
	{
		this.todoListServiceFactory = todoListServiceFactory;
		this.userManager = userManager;
	}

	private async Task<ITodoListService> GetTodoListService()
	{
		var user = await userManager.FindByEmailAsync(this.UserEmail).ConfigureAwait(true);
		return todoListServiceFactory.Create(user);
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Produces(ContentTypes.ApplicationJson)]
	public async Task<ActionResult<TodoListResponse>> GetTodos()
	{
		var todoService = await GetTodoListService().ConfigureAwait(false);
		return (TodoListResponse)await todoService.GetAllAsync().ConfigureAwait(false);
	}

	[HttpPost]
	public async Task<ActionResult<TodoModel>> CreateTodo([FromBody] CreateTodoRequest createTodoRequest)
	{
		if (this.ModelState.IsValid == false)
		{
			return this.BadRequest(this.ModelState);
		}

		var todoListService = await GetTodoListService().ConfigureAwait(true);
		var todo = await todoListService.CreateAsync(new(createTodoRequest.Name)).ConfigureAwait(false);

		return (TodoModel)todo;
	}

	[HttpPatch]
	public async Task<ActionResult<TodoModel>> UpdateTodo([FromBody] TodoModel todoModel)
	{
		if (this.ModelState.IsValid == false)
		{
			return this.BadRequest(this.ModelState);
		}

		var todoListService = await GetTodoListService().ConfigureAwait(false);
		var todo = await todoListService.UpdateAsync(TodoModel.Convert(todoModel, todoListService.Author.Id))
		   .ConfigureAwait(false);
		return (TodoModel)todo;
	}

	[HttpDelete("{todoId:guid}")]
	public async Task<ActionResult> DeleteTodo(Guid todoId)
	{
		if (this.ModelState.IsValid == false)
		{
			return this.BadRequest(this.ModelState);
		}

		var todoListService = await GetTodoListService().ConfigureAwait(false);
		await todoListService.DeleteAsync(todoId).ConfigureAwait(false);
		return this.NoContent();
	}
}
