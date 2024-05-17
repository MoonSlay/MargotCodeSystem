using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MargotCodeSystem.Models.Accounts
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is requred")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember me :)")]
        public bool RememberMe { get; set; }
    }
}
