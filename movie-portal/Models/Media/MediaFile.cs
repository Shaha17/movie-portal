using System;
using System.ComponentModel.DataAnnotations;
using movie_portal.Models.Account;

namespace movie_portal.Models.Media
{
	public class MediaFile
	{
		[Key]
		public Guid Id { get; set; }
		public string FileName { get; set; }
		public DateTime InsertDate { get; set; }
		public Guid MovieId { get; set; }
		public Guid InsertedUserId { get; set; }
		public uint Views { get; set; }
		public bool IsDelete { get; set; }


		public virtual Movie Movie { get; set; }
		public virtual User InsertedUser { get; set; }
	}
}