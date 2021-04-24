using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_portal.Models.Account
{
	public class LoginDTO
	{
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
