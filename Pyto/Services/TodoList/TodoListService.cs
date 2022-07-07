using Pyto.Data.Todo;
using Pyto.Models;

namespace Pyto.Services.TodoList;

public interface ITodoListService
{
	Task<Todo> CreateAsync(Todo todo);
	Task<Models.TodoList> GetAllAsync();

	Task DeleteAsync(Todo todo);
}

public class TodoListService : ITodoListService
{
	private readonly UserDbo author;
	// private readonly ITodo

	public TodoListService(UserDbo author)
	{
		this.author = author;
	}

	public Task<Todo> CreateAsync(Todo todo)
	{
		throw new NotImplementedException();
	}

	public Task<Models.TodoList> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	public Task DeleteAsync(Todo todo)
	{
		throw new NotImplementedException();
	}
}
