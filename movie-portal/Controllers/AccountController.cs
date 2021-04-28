using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using movie_portal.Models.Account;

namespace movie_portal.Controllers
{
	public class AccountController : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public AccountController(ILogger<AccountController> logger, SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public IActionResult AccessDenied()
		{
			return RedirectToAction("Index", "Home");
		}
		[HttpGet]
		public IActionResult Login([FromQuery] string returnUrl)
		{
			return View(new LoginDTO { ReturnUrl = returnUrl });
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("Ошбика", "Имя пользователя или пароль не верны.");
				return View(model);
			}

			if (!string.IsNullOrEmpty(model.ReturnUrl))
			{
				return LocalRedirect(model.ReturnUrl);
			}

			return RedirectToAction("Index", "Home");
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var rez = await _userManager.CreateAsync(new User
			{
				UserName = model.UserName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
			}, model.Password);

			if (!rez.Succeeded)
			{
				foreach (var error in rez.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}
	}
}