using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FileManagerAPI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Введите правильный Имейл")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
