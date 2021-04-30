using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace movie_portal.Models.Account
{
	public class UserDTO
	{
		public Guid Id { get; set; }
		[Required]
		[Display(Name = "Логин")]
		public string UserName { get; set; }
		[Required]
		[EmailAddress]
		[Display(Name = "Почта")]
		public string Email { get; set; }
		[Phone]
		[Display(Name = "Номер телефона")]
		public string PhoneNumber { get; set; }
		[Required]
		[Display(Name = "Пароль")]
		public string Password { get; set; }
		[Display(Name = "Подтверждение пароля")]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }
		[Display(Name = "Новый пароль")]
		public string NewPassword { get; set; }
	}
}
