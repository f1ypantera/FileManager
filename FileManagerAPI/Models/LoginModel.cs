using System.ComponentModel.DataAnnotations;

namespace FileManagerAPI.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
