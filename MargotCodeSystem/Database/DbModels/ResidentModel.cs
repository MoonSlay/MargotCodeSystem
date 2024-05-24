using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MargotCodeSystem.Models.Identity;

namespace MargotCodeSystem.Database.DbModels
{
    public class ResidentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? Fullname { get; set; }

        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string? PresentAddress { get; set; }

        public string? HouseType { get; set; }

        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string? ProvincialAddress { get; set; }

        public string? LengthOfStay { get; set; }

        public string? Height { get; set; }

        public string? Weight { get; set; }

        public string? Gender { get; set; }

        public string? CivilStatus { get; set; }

        public int? ContactNumber { get; set; }

        public string? DateOfBirth { get; set; }

        public string? PlaceOfBirth { get; set; }

        public string? Religion { get; set; }

        public string? Email { get; set; }

        public string? Registered { get; set; }

        public int? PrecintNumber { get; set; }

        public string? Remarks { get; set; }

        public bool? SeniorCitizen { get; set; }

        public bool? StreetSweeper { get; set; }

        public bool? ActiveResident { get; set; }

        public bool? TakingMeds { get; set; }

        public string? Meds { get; set; }

        public bool? PetOwner { get; set; }

        public string? Pets { get; set; }

        public string? EmployeeDuration { get; set; }

        public string? CompanyName { get; set; }

        public string? Employer { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? UserId { get; set; }
    }
}
