namespace Pyto.Models;

public enum TodoState
{
	Unchecked = 1,
	Checked = 2,
}


public record Todo(Guid Id, string Content, TodoState State, Guid AuthorId)
{
	public static Todo Create(string name, Guid authorId, TodoState state = TodoState.Unchecked)
	{
		return new Todo(Guid.NewGuid(), name, state, authorId);
	}
}
