﻿#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pyto.Data.Users;

namespace Pyto.Data;

public class ApplicationDbContext : IdentityDbContext<UserDbo, IdentityRole<Guid>, Guid>
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<TodoDbo> Todos { get; set; }
}
