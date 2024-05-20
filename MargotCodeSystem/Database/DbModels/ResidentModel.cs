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

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")] 

        public string PresentAddress { get; set; }

        [Required]
        public string HouseType { get; set; }

        [Required]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")] 
        public string ProvincialAddress { get; set; }

        [Required]
        public string LengthOfStay { get; set; }
        [Required]
        public string Height { get; set; }
        [Required]
        public string Weight { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string CivilStatus { get; set; }

        [Required]
        public int ContactNumber { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; }

        [Required]
        public string Religion { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public string Registered { get; set; }

        [Required]
        public int PrecintNumber { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public bool SeniorCitizen { get; set; }

        [Required]
        public bool StreetSweeper { get; set; }

        [Required]
        public bool ActiveResident { get; set; }

        [Required]
        public bool TakingMeds { get; set; }

        [Required]
        public string Meds { get; set; }

        [Required]
        public bool PetOwner { get; set; }

        [Required]
        public string Pets { get; set; }

        [Required]
        public string EmployeeDuration { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Employer { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
