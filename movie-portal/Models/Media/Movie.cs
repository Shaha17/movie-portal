using System.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using movie_portal.Models.Account;

namespace movie_portal.Models.Media
{
	public class Movie
	{
		public Movie()
		{
			this.Genres = new List<Genre>();
		}

		[Key]
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Director { get; set; }
		public string Description { get; set; }
		public uint ReleaseYear { get; set; }
		public string ImageFileName { get; set; }


		public string InsertUserId { get; set; }
		public DateTime InsertDateTime { get; set; }
		public DateTime? UpdateDateTime { get; set; }
		public bool IsDelete { get; set; }
		public uint Views { get; set; }



		[ForeignKey("InsertUserId")]
		public virtual User User { get; set; }


		public virtual ICollection<Comment> Comments { get; set; }
		public virtual ICollection<Genre> Genres { get; set; }
		public virtual ICollection<MediaFile> MediaFiles { get; set; }


		
	}
}