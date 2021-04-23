using System.ComponentModel.DataAnnotations;
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using movie_portal.Models.Media;

namespace movie_portal.Models.Account
{
	public class User : IdentityUser
	{
		public string AvatarFileName { get; set; }
		public virtual ICollection<Movie> Movies { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}