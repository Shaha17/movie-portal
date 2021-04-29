using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using movie_portal.Models.Account;

namespace movie_portal.Models.Media
{
	public class MediaDTO
	{
		public Guid Id { get; set; }
		[Display(Name = "Название")]
		[Required]
		public string Title { get; set; }
		[Display(Name = "Описание")]
		[Required]
		public string Description { get; set; }
		[Display(Name = "Режисёр")]
		public string Director { get; set; }
		[Display(Name = "Год выпуска")]
		[Required]
		public uint ReleaseYear { get; set; }
		[Display(Name = "Жарны")]
		// [Required]
		public List<Guid> GenresId { get; set; }
		public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
		public string InsertUserId { get; set; }
		public DateTime InsertDateTime { get; set; }

		public string ImageFileName { get; set; }
		[Display(Name = "Фотография")]
		public IFormFile ImageFile { get; set; }

		public List<string> MediaFilesName { get; set; }
		public IFormFileCollection MediaFiles { get; set; }

		public bool IsDelete { get; set; }
		public DateTime? UpdateDateTime { get; set; }
		public uint Views { get; set; } = 0;

		public User User { get; set; }
		public List<Comment> Comments { get; set; } = new List<Comment>();

	}
}