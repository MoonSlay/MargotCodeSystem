using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MargotCodeSystem.Models.Identity;
using MargotCodeSystem.Database.DbModels.ResidentModels;

namespace MargotCodeSystem.Database.DbModels
{
    public class ResidentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        public string MiddleName { get; set; }
        public string? Fullname { get; set; }

        [Required(ErrorMessage = "Present Address is required")]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string PresentAddress { get; set; }

        [Required(ErrorMessage = "House Type is required")]
        public string HouseType { get; set; }

        [Required(ErrorMessage = "Provincial Address is required")]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string ProvincialAddress { get; set; }

        [Required(ErrorMessage = "Length of Stay is required")]
        public string LengthOfStay { get; set; }

        [Required(ErrorMessage = "Height is required")]
        public string Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Civil Status is required")]
        public string CivilStatus { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public int ContactNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Place of Birth is required")]
        public string PlaceOfBirth { get; set; }

        [Required(ErrorMessage = "Religion is required")]
        public string Religion { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Registered is required")]
        public string Registered { get; set; }

        [Required(ErrorMessage = "Precint Number is required")]
        public int PrecintNumber { get; set; }

        public string? Remarks { get; set; }
        public bool SeniorCitizen { get; set; } = false;
        public bool StreetSweeper { get; set; } = false;
        public bool ActiveResident { get; set; } = false;
        public bool TakingMeds { get; set; } = false;

        public ICollection<MedsModel> Meds { get; set; }

        public bool PetOwner { get; set; } = false;

        public ICollection<PetModel> Pets { get; set; }

        public string? EmployeeDuration { get; set; }

        public string? CompanyName { get; set; }

        public string? Employer { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? UserId { get; set; }

        public ResidentModel()
        {
            Pets = new List<PetModel>();
            Meds = new List<MedsModel>();
            // Initialize other collections if needed
        }
    }
}
