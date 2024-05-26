using MargotCodeSystem.Utils;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        private string? _lastName;
        [Required]
        public string LastName
        {
            get => EncryptionHelper.DecryptString(_lastName);
            set => _lastName = EncryptionHelper.EncryptString(value);
        }

        private string? _firstName;
        [Required]
        public string FirstName
        {
            get => EncryptionHelper.DecryptString(_firstName);
            set => _firstName = EncryptionHelper.EncryptString(value);
        }
    }
}
