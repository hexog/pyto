#nullable disable
namespace Pyto.Controllers.Models;

public class ErrorResponse
{
	public ErrorResponse()
	{
	}

	public ErrorResponse(string message)
	{
		Message = message;
	}

	public string Message { get; set; }
}
