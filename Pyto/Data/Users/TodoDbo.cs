#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pyto.Data.Users;

[Index(nameof(Content))]
public class TodoDbo
{
	[Key]
	public Guid Id { get; set; }
	[Required]
	public string Content { get; set; }

	[Required]
	public DateTime Created { get; set; }
	[Required]
	public DateTime Updated { get; set; }

	public UserDbo Author { get; set; }
	[Required]
	public Guid AuthorId { get; set; }
}
