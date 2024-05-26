using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MargotCodeSystem.Utils;

namespace MargotCodeSystem.Models.Accounts
{
    public class LoginModel
    {
        private string? _username;
        [Required(ErrorMessage = "Username is required")]
        public string Username
        {
            get => EncryptionHelper.DecryptString(_username);
            set => _username = EncryptionHelper.EncryptString(value);
        }

        private string? _password;
        [Required(ErrorMessage = "Password is requred")]
        [DataType(DataType.Password)]
        public string Password
        {
            get => EncryptionHelper.DecryptString(_password);
            set => _password = EncryptionHelper.EncryptString(value);
        }
        [DisplayName("Remember me :)")]
        public bool RememberMe { get; set; }
    }
}
