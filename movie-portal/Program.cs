using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using movie_portal.Context;
using movie_portal.Models.Account;

namespace movie_portal
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			using var scope = host.Services.CreateScope();
			var services = scope.ServiceProvider;
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
			try
			{
				var context = services.GetRequiredService<MoviePortalContext>();
				var userManager = services.GetRequiredService<UserManager<User>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				var webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();

				if (!Directory.Exists(Path.Combine(webHostEnvironment.WebRootPath, "media")))
				{
					Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, "media"));
				}
				if (!Directory.Exists(Path.Combine(webHostEnvironment.WebRootPath, "images")))
				{
					Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, "images"));
				}

				await context.Database.MigrateAsync();

				await ContextHelper.Seeding(context, userManager, roleManager);


				logger.LogInformation("Migrate successfull");
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			// await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
