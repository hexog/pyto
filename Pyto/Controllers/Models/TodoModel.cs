#nullable disable
using Pyto.Models;

namespace Pyto.Controllers.Models;

public class TodoModel
{
	public Guid Id { get; set; }
	public string Content { get; set; }
	public TodoState State { get; set; } // TODO: enum validation

	public static explicit operator TodoModel(Todo todo)
	{
		return new TodoModel
		{
			Content = todo.Content,
			Id = todo.Id,
			State = todo.State,
		};
	}

	public static Todo Convert(TodoModel todoModel, Guid userId)
	{
		return new Todo(todoModel.Id,
						todoModel.Content,
						todoModel.State,
						userId);
	}
}
