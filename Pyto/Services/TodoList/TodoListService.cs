using Pyto.Controllers.Models;
using Pyto.Data.Todo;
using Pyto.Data.Users;
using Pyto.Models;
using Pyto.Services.Exceptions;

namespace Pyto.Services.TodoList;

public record TodoParameters(string Content);

public interface ITodoListService
{
	public UserDbo Author { get; }
	Task<Todo> CreateAsync(TodoParameters parameters);
	Task<Models.TodoList> GetAllAsync();

	Task DeleteAsync(Todo todo);
	Task<Todo> UpdateAsync(Todo todo);
	Task DeleteAsync(Guid todoId);
}

public class TodoListService : ITodoListService
{
	public UserDbo Author { get; }
	private readonly ITodoRepository todoRepository;

	public TodoListService(UserDbo author, ITodoRepository todoRepository)
	{
		this.Author = author;
		this.todoRepository = todoRepository;
	}

	public async Task<Todo> CreateAsync(TodoParameters parameters)
	{
		var todo = new Todo(Guid.NewGuid(),
							parameters.Content,
							TodoState.Unchecked,
							Author.Id);

		await todoRepository.AddAsync(todo).ConfigureAwait(false);
		return todo;
	}

	public async Task<Models.TodoList> GetAllAsync()
	{
		var todos = await todoRepository.FindByUserIdAsync(Author.Id).ConfigureAwait(false);
		return new Models.TodoList(todos);
	}

	public Task DeleteAsync(Todo todo)
	{
		return DeleteAsync(todo.Id);
	}

	public async Task<Todo> UpdateAsync(Todo todo)
	{
		todo = await todoRepository.ReadAsync(todo.Id).ConfigureAwait(false);
		if (todo.AuthorId != Author.Id)
		{
			throw new ForbiddenException();
		}
		todo = await todoRepository.UpdateAsync(todo).ConfigureAwait(false);
		return todo;
	}

	public async Task DeleteAsync(Guid todoId)
	{
		var todo = await todoRepository.ReadAsync(todoId).ConfigureAwait(false);
		if (todo.AuthorId != Author.Id)
		{
			throw new ForbiddenException();
		}

		await DeleteAsync(todo).ConfigureAwait(false);
	}
}
