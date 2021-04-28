using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using movie_portal.Models.Media;

namespace movie_portal.Models.Account
{
	public class Comment
	{
		[Key]
		public Guid Id { get; set; }
		public string UserId { get; set; }
		public Guid MovieId { get; set; }
		public string Content { get; set; }
		public DateTime UploadDate { get; set; }
		public bool IsDeleted { get; set; } = false;




		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		[ForeignKey("MovieId")]
		public virtual Movie Movie { get; set; }

	}
}