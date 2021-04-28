using System.Security.AccessControl;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using movie_portal.Models;
using Microsoft.AspNetCore.Authorization;
using movie_portal.Models.Media;
using movie_portal.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace movie_portal.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly MoviePortalContext _moviePortalContext;
		private readonly IMapper _mapper;

		public HomeController(ILogger<HomeController> logger, movie_portal.Context.MoviePortalContext moviePortalContext, IMapper mapper)
		{
			_logger = logger;
			_moviePortalContext = moviePortalContext;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var lst = (await _moviePortalContext.Movies.ToListAsync());
			var dtoLst = lst.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
			return View(dtoLst);
		}

		[Authorize]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
