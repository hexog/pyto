using Pyto.Controllers.Models;
using Pyto.Data.Todo;
using Pyto.Data.Users;
using Pyto.Models;

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
		// todo: delete only user's todo
		return todoRepository.RemoveAsync(todo);
	}

	public async Task<Todo> UpdateAsync(Todo todo)
	{
		// todo: update only user's todo
		todo = await todoRepository.UpdateAsync(todo).ConfigureAwait(false);
		return todo;
	}

	public async Task DeleteAsync(Guid todoId)
	{
		// todo: delete only user's todo
		var todo = await todoRepository.ReadAsync(todoId).ConfigureAwait(false);
		await DeleteAsync(todo).ConfigureAwait(false);
	}
}
