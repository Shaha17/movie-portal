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
using X.PagedList;

namespace movie_portal.Controllers
{
	public class GenreController : Controller
	{
		private readonly MoviePortalContext _moviePortalContext;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<User> _userManager;
		// private readonly int pagesize = 1;
		private readonly int pagesize = 12;


		public GenreController(MoviePortalContext moviePortalContext, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_moviePortalContext = moviePortalContext;
			_webHostEnvironment = webHostEnvironment;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? page)
		{
			int pageNumber = page ?? 1;

			var lst = (await _moviePortalContext.Genres.Where(g => g.IsDelete == false).ToListAsync());
			var dtoLst = lst.Select(g => _mapper.Map<GenreDTO>(g)).ToList();
			return View(dtoLst.ToPagedList(pageNumber, this.pagesize));
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> List()
		{
			var lst = (await _moviePortalContext.Genres.Where(g => g.IsDelete == false).ToListAsync());
			var dtoLst = lst.Select(g => _mapper.Map<GenreDTO>(g)).ToList();
			return View(dtoLst);
		}


		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			var genreDTO = new GenreDTO();
			return View(genreDTO);
		}
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(GenreDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var currenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (_moviePortalContext.Genres.Any(g => g.Name.ToLower().Equals(model.Name.ToLower())))
			{
				ModelState.AddModelError("", "Жанр с таким названием уже есть в базе");
				return View(model);
			}

			var genre = _mapper.Map<Genre>(model);

			_moviePortalContext.Genres.Add(genre);
			await _moviePortalContext.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(string id)
		{
			var guidId = Guid.Parse(id);
			var genre = await _moviePortalContext.Genres.FirstOrDefaultAsync(g => g.Id.Equals(guidId));

			var genreDTO = _mapper.Map<GenreDTO>(genre);

			return View(genreDTO);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return RedirectToAction("List", "Genre");
			}
			if (!Guid.TryParse(id, out Guid guidId))
			{
				return RedirectToAction("List", "Genre");
			}

			var genre = await _moviePortalContext.Genres.FirstOrDefaultAsync(g => g.Id.Equals(guidId));
			if (genre == null)
			{
				return RedirectToAction("List", "Genre");
			}

			genre.IsDelete = true;
			await _moviePortalContext.SaveChangesAsync();

			return RedirectToAction("List", "Genre");
		}
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(GenreDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var genre = _moviePortalContext.Genres.Find(model.Id);

			if (genre == null)
			{
				return RedirectToAction("Index", "Genre");
			}

			genre.Name = model.Name;

			await _moviePortalContext.SaveChangesAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}