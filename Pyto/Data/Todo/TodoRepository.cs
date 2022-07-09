using Microsoft.EntityFrameworkCore;
using Pyto.Models;

namespace Pyto.Data.Todo;

public interface ITodoRepository
{
	Task AddAsync(Models.Todo todo);
	Task<IList<Models.Todo>> FindByUserIdAsync(Guid userId);
	Task<Models.Todo> UpdateAsync(Models.Todo todo);
	Task RemoveAsync(Models.Todo todo);
	Task<Models.Todo> ReadAsync(Guid todoId);
}

public class TodoRepository : ITodoRepository
{
	private readonly ApplicationDbContext dbContext;

	public TodoRepository(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public Task AddAsync(Models.Todo todo)
	{
		var todoDbo = ConvertToDbo(todo);
		dbContext.Todos.Add(todoDbo);
		return dbContext.SaveChangesAsync();
	}

	public async Task<IList<Models.Todo>> FindByUserIdAsync(Guid userId)
	{
		var todosDbo = await dbContext.Todos
		   .Where(x => x.AuthorId == userId)
		   .ToListAsync()
		   .ConfigureAwait(false);

		return todosDbo.Select(ConvertToDto).ToList();
	}

	public async Task<Models.Todo> UpdateAsync(Models.Todo todo)
	{
		var dbo = ConvertToDbo(todo);
		dbo = dbContext.Update(dbo).Entity;
		await dbContext.SaveChangesAsync().ConfigureAwait(false);
		return ConvertToDto(dbo);
	}

	public Task RemoveAsync(Models.Todo todo)
	{
		var dbo = ConvertToDbo(todo);
		dbContext.Remove(dbo);
		return dbContext.SaveChangesAsync();
	}

	public async Task<Models.Todo> ReadAsync(Guid todoId)
	{
		var todo = await dbContext.Todos.FindAsync(todoId).ConfigureAwait(false);

		if (todo is null)
		{
			throw new InvalidOperationException($"Could not find Todo with id: {todoId}");
		}

		return ConvertToDto(todo);
	}

	private static TodoDbo ConvertToDbo(Models.Todo todo)
	{
		var todoDbo = new TodoDbo
		{
			AuthorId = todo.AuthorId,
			Content = todo.Content,
			Id = todo.Id,
			Created = DateTime.UtcNow,
			State = todo.State,
			Updated = DateTime.UtcNow,
		};
		return todoDbo;
	}

	private static Models.Todo ConvertToDto(TodoDbo todo)
	{
		return new Models.Todo(todo.Id,
							   todo.Content,
							   todo.State,
							   todo.AuthorId);
	}
}
