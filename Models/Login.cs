using System.ComponentModel.DataAnnotations;

namespace DigitalPhotoPrintingSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email / Username is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}