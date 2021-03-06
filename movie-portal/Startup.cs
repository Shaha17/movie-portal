using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using movie_portal.Context;
using movie_portal.Models.Account;
using movie_portal.Services;

namespace movie_portal
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<FormOptions>(options =>
			{
				// options.ValueCountLimit = int.MinValue;
				options.ValueLengthLimit = 1024 * 1024 * 1024 * 1;
				options.MultipartBodyLengthLimit = long.MaxValue;

			});

			services.AddControllersWithViews();

			services.AddDbContext<MoviePortalContext>(option =>
			{
				option.UseSqlServer(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies();
			});

			services.AddIdentity<User, IdentityRole>(option =>
			{
				option.User.AllowedUserNameCharacters = null;
			}).AddRoleManager<RoleManager<IdentityRole>>()
			  .AddUserManager<UserManager<User>>()
			  .AddEntityFrameworkStores<MoviePortalContext>()
			  .AddDefaultTokenProviders();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

			services.AddAuthorization();


			services.Configure<IdentityOptions>(option =>
			{
				option.Password.RequireDigit = false;
				option.Password.RequireLowercase = false;
				option.Password.RequireUppercase = false;
				option.Password.RequireNonAlphanumeric = false;
				option.Password.RequiredLength = 4;
				option.User.RequireUniqueEmail = true;
			});

			services.ConfigureApplicationCookie(option =>
			{
				option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				option.SlidingExpiration = true;
			});

			services.InitServices();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
