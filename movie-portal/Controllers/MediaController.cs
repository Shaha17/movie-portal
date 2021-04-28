using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using movie_portal.Context;
using movie_portal.Models.Account;
using movie_portal.Models.Media;

namespace movie_portal.Controllers
{
	public class MediaController : Controller
	{
		private readonly MoviePortalContext _moviePortalContext;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<User> _userManager;


		public MediaController(MoviePortalContext moviePortalContext, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_moviePortalContext = moviePortalContext;
			_webHostEnvironment = webHostEnvironment;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(string genreId)
		{
			var lst = new List<Movie>();
			if (string.IsNullOrEmpty(genreId))
			{
				lst = (await _moviePortalContext.Movies.Where(m => m.IsDelete == false).ToListAsync());
			}
			else
			{
				var guidGenreId = Guid.Parse(genreId);
				lst = (await _moviePortalContext.Movies
					.Where(m => m.IsDelete == false)
					.Where(m => m.Genres.Any(g => g.Id.Equals(guidGenreId)))
					.ToListAsync());
			}
			var dtoLst = lst.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
			return View(dtoLst);
		}
		[Authorize]
		public async Task<IActionResult> AddComment(string id, string content)
		{

			var guidUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var movieId = id;
			if (string.IsNullOrEmpty(content))
			{
				return BadRequest();
			}

			var movie = await _moviePortalContext.Movies.FirstOrDefaultAsync(m => m.Id.Equals(Guid.Parse(id)));

			if (movie == null)
			{
				return BadRequest();
			}
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(guidUserId));
			if (user == null)
			{
				return BadRequest();
			}
			var comment = new Comment()
			{
				Id = Guid.NewGuid(),
				User = user,
				Movie = movie,
				Content = content,
				UploadDate = DateTime.Now,
			};

			_moviePortalContext.Comments.Add(comment);

			await _moviePortalContext.SaveChangesAsync();

			return Ok("ok");
		}
		[Authorize]
		public async Task<IActionResult> DeleteComment(string commentId)
		{
			var guidUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(guidUserId));
			if (user == null)
			{
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { errorMessage = $"Can't find user with this Id: {guidUserId}" });
			}

			var comment = await _moviePortalContext.Comments.FirstOrDefaultAsync(c => c.Id.Equals(Guid.Parse(commentId)));
			if (comment == null)
			{
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { errorMessage = $"Can't find comment with this Id: {commentId}" });
			}

			if (comment.UserId != user.Id)
			{
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { errorMessage = $"Comment doesn't belong you" });
			}

			comment.IsDeleted = true;

			await _moviePortalContext.SaveChangesAsync();

			return Ok("ok");
		}


		[HttpGet("Details/{id}")]
		public async Task<IActionResult> Details([FromRoute] Guid id)
		{
			var movie = await _moviePortalContext.Movies.FindAsync(id);

			if (movie == null)
			{
				return RedirectToAction("Index", "Home");
			}
			movie.Views++;
			var mediaDTO = _mapper.Map<MediaDTO>(movie);
			mediaDTO.Genres = movie.Genres.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
			mediaDTO.Comments = movie.Comments.OrderByDescending(c => c.UploadDate).ToList();
			mediaDTO.MediaFilesName = movie.MediaFiles.OrderByDescending(f => f.InsertDate).Select(p => p.FileName).ToList();

			await _moviePortalContext.SaveChangesAsync();

			return View(mediaDTO);
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var movieDTO = new MediaDTO();
			var genres = await _moviePortalContext.Genres.Where(g => g.IsDelete == false).Select(p => new SelectListItem
			{
				Value = p.Id.ToString(),
				Text = p.Name
			}).ToListAsync();

			movieDTO.ReleaseYear = 2020;
			movieDTO.Genres = genres;

			return View(movieDTO);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024 * 1024)]
		public async Task<IActionResult> Create(MediaDTO model)
		{

			if (!ModelState.IsValid)
			{
				model.MediaFiles = null;
				model.ImageFile = null;
				return View(model);
			}

			string finalFileName = null;

			if (model.ImageFile != null)
			{
				finalFileName = await CopyImageFile(model.ImageFile);
			}

			// if (model.Id != null)
			// {

			// }

			//Current user id
			var currenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var genresFromModel = new List<Genre>();
			if (model.GenresId != null)
			{
				genresFromModel = await _moviePortalContext.Genres.Where(x => model.GenresId.Contains(x.Id)).ToListAsync();
			}

			var movie = new Movie
			{
				Genres = genresFromModel,
				Description = model.Description,
				Director = model.Director,
				Id = Guid.NewGuid(),
				ImageFileName = finalFileName,
				InsertDateTime = DateTime.Now,
				InsertUserId = currenUserId,
				IsDelete = false,
				ReleaseYear = model.ReleaseYear,
				Title = model.Title,
			};

			foreach (var file in model.MediaFiles)
			{
				var mediaFile = new MediaFile();

				//  mediaFile.FileName = await CopyMediaFile(file);
				mediaFile.FileName = await CopyMediaFile(file);
				mediaFile.Id = Guid.NewGuid();
				mediaFile.InsertDate = DateTime.Now;
				mediaFile.InsertedUserId = currenUserId;
				mediaFile.IsDelete = false;
				mediaFile.Movie = movie;
				mediaFile.MovieId = movie.Id;
				mediaFile.Views = 0;

				_moviePortalContext.MediaFiles.Add(mediaFile);
			}

			_moviePortalContext.Movies.Add(movie);
			await _moviePortalContext.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
		}

		[Authorize(Roles = "Admin")]
		[NonAction]
		private async Task<string> CopyImageFile(IFormFile imageFile)
		{
			if (imageFile == null) return null;

			var rootPath = _webHostEnvironment.WebRootPath;
			var filename = Path.GetFileNameWithoutExtension(imageFile.FileName); //02animalpicture
			var fileExtension = Path.GetExtension(imageFile.FileName); //.jpeg
			var finalFileName = $"{filename}_{DateTime.Now.ToString("yyMMddHHmmssff")}{fileExtension}";
			var filePath = Path.Combine(rootPath, "images", finalFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await imageFile.CopyToAsync(fileStream);
			}
			return finalFileName;
		}

		[Authorize(Roles = "Admin")]
		[NonAction]
		private async Task<string> CopyMediaFile(IFormFile mediaFile)
		{
			if (mediaFile == null) return null;

			var rootPath = _webHostEnvironment.WebRootPath;
			var filename = Path.GetFileNameWithoutExtension(mediaFile.FileName); //02animalpicture
			var fileExtension = Path.GetExtension(mediaFile.FileName); //.jpeg
			var finalFileName = $"{filename}_{DateTime.Now.ToString("yyMMddHHmmssff")}{fileExtension}";
			var filePath = Path.Combine(rootPath, "media", finalFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await mediaFile.CopyToAsync(fileStream);
			}
			return finalFileName;
		}


	}
}