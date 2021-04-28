using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using movie_portal.Models.Account;
using movie_portal.Models.Media;

namespace movie_portal.Context
{
	public class MoviePortalContext : IdentityDbContext<User>
	{
		public MoviePortalContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Comment> Comments { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<MediaFile> MediaFiles { get; set; }
		public DbSet<Genre> Genres { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable("Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
		}
	}
}