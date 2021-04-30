using System.Text.RegularExpressions;
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
		[Required]
		public string UserName { get; set; }
		[Display(Name = "Пароль")]
		[Required]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
