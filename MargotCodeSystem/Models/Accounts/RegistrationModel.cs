using MargotCodeSystem.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Models.Accounts
{
    public class RegistrationModel
    {
        private string _email;
        [Required]
        [EmailAddress]
        public string Email
        {
            get => EncryptionHelper.DecryptString(_email);
            set => _email = EncryptionHelper.EncryptString(value);
        }

        private string _username;
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and max {1} characters long", MinimumLength = 3)]
        public string Username
        {
            get => EncryptionHelper.DecryptString(_username);
            set => _username = EncryptionHelper.EncryptString(value);
        }

        private string _lastName;
        [Required]
        [DisplayName("Last Name")]
        public string LastName
        {
            get => EncryptionHelper.DecryptString(_lastName);
            set => _lastName = EncryptionHelper.EncryptString(value);
        }

        private string _firstName;
        [Required]
        [DisplayName("First Name")]
        public string FirstName
        {
            get => EncryptionHelper.DecryptString(_firstName);
            set => _firstName = EncryptionHelper.EncryptString(value);
        }

        private string _pass;
        [Required]
        [StringLength(100,ErrorMessage = "The {0} must be atleast {2} and max {1} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password
        {
            get => EncryptionHelper.DecryptString(_pass);
            set => _pass = EncryptionHelper.EncryptString(value);
        }

        private string _confirmPass;
        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword
        {
            get => EncryptionHelper.DecryptString(_confirmPass);
            set => _confirmPass = EncryptionHelper.EncryptString(value);
        }
    }
}
