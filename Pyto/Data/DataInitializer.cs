using Microsoft.AspNetCore.Identity;
using Pyto.Controllers.Authorization;

namespace Pyto.Data;

public static class DataInitializer
{
	public static async Task InitializeRoles(IServiceProvider serviceProvider)
	{


		using var serviceScope = serviceProvider.CreateScope();
		var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
		if (!await roleManager.RoleExistsAsync(Roles.Administrator).ConfigureAwait(false))
			await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Administrator)).ConfigureAwait(false);
		if (!await roleManager.RoleExistsAsync(Roles.User).ConfigureAwait(false))
			await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.User)).ConfigureAwait(false);


	}
}
