#nullable disable
using Pyto.Models;

namespace Pyto.Controllers.Models;

public class TodoListResponse
{
	public TodoModel[] TodoList { get; set; }

	public static explicit operator TodoListResponse(TodoList todoList)
	{
		return new TodoListResponse
		{
			TodoList = todoList.Todos.Select(x => (TodoModel) x).ToArray(),
		};
	}
}
