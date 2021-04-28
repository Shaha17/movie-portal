using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace movie_portal.Models.Media
{
	public class Genre
	{
		public Genre()
		{
			this.Movies = new List<Movie>();
		}

		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Movie> Movies { get; set; }

	}
}