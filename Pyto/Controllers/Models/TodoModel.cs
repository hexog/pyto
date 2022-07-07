#nullable disable
using Pyto.Models;

namespace Pyto.Controllers.Models;

public class TodoModel
{
	public Guid Id { get; set; }
	public string Content { get; set; }
	public TodoState TodoState { get; set; }

	public static explicit operator TodoModel(Todo todo)
	{
		return new TodoModel
		{
			Content = todo.Content,
			Id = todo.Id,
			TodoState = todo.State,
		};
	}

	public static Todo Convert(TodoModel todoModel, Guid userId)
	{
		return new Todo(todoModel.Id,
						todoModel.Content,
						todoModel.TodoState,
						userId);
	}
}
