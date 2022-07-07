#nullable disable
using Pyto.Models;

namespace Pyto.Controllers.Models;

public class TodoResponse
{
	public Guid Id { get; set; }
	public string Content { get; set; }
	public TodoState TodoState { get; set; }
}
