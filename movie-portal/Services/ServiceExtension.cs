using System;
using Microsoft.Extensions.DependencyInjection;

namespace movie_portal.Services
{
	public static class ServiceExtension
	{
		public static void InitServices(this IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
		}
	}
}