using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using movie_portal.Models.Account;

namespace movie_portal.Controllers
{
	public class AccountController : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly IMapper _mapper;
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public AccountController(ILogger<AccountController> logger, SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper)
		{
			_mapper = mapper;
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
		public async Task<IActionResult> Register(UserDTO model)
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
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit()
		{
			var currenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(currenUserId))
			{
				return RedirectToAction("Index", "Home");
			}

			var user = await _userManager.FindByIdAsync(currenUserId);

			var userDTO = _mapper.Map<UserDTO>(user);

			return View(userDTO);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(UserDTO model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var currenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!currenUserId.Equals(model.Id.ToString()))
			{
				return RedirectToAction("Login", "Account");
			}
			var user = await _userManager.FindByIdAsync(model.Id.ToString());

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var usernameChangeRez = await _userManager.SetUserNameAsync(user, model.UserName);
			if (!usernameChangeRez.Succeeded)
			{
				foreach (var item in usernameChangeRez.Errors)
				{
					ModelState.AddModelError(item.Code, item.Description);
				}
			}
			var phoneChangeRez = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
			if (!phoneChangeRez.Succeeded)
			{
				foreach (var item in phoneChangeRez.Errors)
				{
					ModelState.AddModelError(item.Code, item.Description);
				}
			}
			var emailChangeRez = await _userManager.SetEmailAsync(user, model.Email);
			if (!emailChangeRez.Succeeded)
			{
				foreach (var item in emailChangeRez.Errors)
				{
					ModelState.AddModelError(item.Code, item.Description);
				}
			}

			if (!string.IsNullOrEmpty(model.Password))
			{
				var passChangeRez = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
				if (!passChangeRez.Succeeded)
				{
					foreach (var item in passChangeRez.Errors)
					{
						ModelState.AddModelError(item.Code, item.Description);
					}
				}
			}

			await _signInManager.SignOutAsync();

			return RedirectToAction("Login", "Account");
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}
	}
}