using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Pyto.Controllers.Models;

public class CreateTodoRequest
{
	[Required] public string Name { get; set; }
}
