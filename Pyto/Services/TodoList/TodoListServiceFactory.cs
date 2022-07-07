using Pyto.Data.Todo;

namespace Pyto.Services.TodoList;

public interface ITodoListServiceFactory
{
	public ITodoListService Create(UserDbo author);
}

public class TodoListServiceFactory : ITodoListServiceFactory
{
	private readonly IServiceProvider serviceProvider;

	public TodoListServiceFactory(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public ITodoListService Create(UserDbo author)
	{
		using var serviceScope = serviceProvider.CreateScope();

		return new TodoListService(author);
	}
}
