using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using movie_portal.Models.Account;
using movie_portal.Models.Media;

namespace movie_portal.Context
{
	public class ContextHelper
	{
		public static async Task Seeding(MoviePortalContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any(p => p.Name.Equals("Admin")))
			{
				var adminRole = new IdentityRole
				{
					Name = "Admin",
					NormalizedName = "ADMIN",
				};
				await roleManager.CreateAsync(adminRole);
			}

			if (!userManager.Users.Any(p => p.UserName.Equals("admin")))
			{
				var adminUser = new User
				{
					UserName = "admin",
					Email = "admin@gmail.com"
				};
				var rez = await userManager.CreateAsync(adminUser, "pass");

				if (rez.Succeeded)
				{
					var adminRole = await roleManager.FindByNameAsync("Admin");

					await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Admin"), adminRole.Name);
				}
			}

			if (!context.MediaGenres.Any())
			{
				var mediaGenres = new List<MediaGenre>
				{
					new MediaGenre { Id = Guid.NewGuid(), Name = "Хоррор"},
					new MediaGenre { Id = Guid.NewGuid(), Name = "Фантастика"},
					new MediaGenre { Id = Guid.NewGuid(), Name = "Документальные"},
					new MediaGenre { Id = Guid.NewGuid(), Name = "Комедии"},
				};

				context.MediaGenres.AddRange(mediaGenres);
				await context.SaveChangesAsync();
			}
		}
	}
}