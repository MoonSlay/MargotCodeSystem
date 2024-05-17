using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
    }
}
