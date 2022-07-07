namespace Pyto.Models;

public enum TodoState
{
	Unchecked = 1,
	Checked = 2,
}

public record Todo(Guid Id, string Name, TodoState State)
{
	public static Todo Create(string name, TodoState state = TodoState.Unchecked)
	{
		return new Todo(Guid.NewGuid(), name, state);
	}
}
