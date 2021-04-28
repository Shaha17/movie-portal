using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace movie_portal.Models.Media
{
	public class GenreDTO
	{

		public Guid Id { get; set; }
		[Display(Name = "Название")]
		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public ICollection<Movie> Movies { get; set; }

	}
}