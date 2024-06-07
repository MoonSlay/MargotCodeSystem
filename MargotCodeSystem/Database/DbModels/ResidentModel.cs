using MargotCodeSystem.Database.DbModels.ResidentModels;
using MargotCodeSystem.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MargotCodeSystem.Database.DbModels
{
    public class ResidentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _lastName;
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName
        {
            get => EncryptionHelper.DecryptString(_lastName);
            set => _lastName = EncryptionHelper.EncryptString(value);
        }

        private string _firstName;
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName
        {
            get => EncryptionHelper.DecryptString(_firstName);
            set => _firstName = EncryptionHelper.EncryptString(value);
        }

        private string _middleName;
        public string? MiddleName
        {
            get => EncryptionHelper.DecryptString(_middleName);
            set => _middleName = EncryptionHelper.EncryptString(value);
        }

        private string _fullName;
        public string? Fullname
        {
            get => EncryptionHelper.DecryptString(_fullName);
            set => _fullName = EncryptionHelper.EncryptString(value);
        }

        private string _presentAddress;
        [Required(ErrorMessage = "Present Address is required")]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string PresentAddress
        {
            get => EncryptionHelper.DecryptString(_presentAddress);
            set => _presentAddress = EncryptionHelper.EncryptString(value);
        }

        private string _houseType;
        [Required(ErrorMessage = "House Type is required")]
        public string HouseType
        {
            get => EncryptionHelper.DecryptString(_houseType);
            set => _houseType = EncryptionHelper.EncryptString(value);
        }

        private string _provincialAddress;
        [Required(ErrorMessage = "Provincial Address is required")]
        [RegularExpression(@"^.*,\s.*,\s.*,\s.*,\s.*$", ErrorMessage = "Please enter the address in the format: House# , street , baranggay , city , province")]
        public string ProvincialAddress
        {
            get => EncryptionHelper.DecryptString(_provincialAddress);
            set => _provincialAddress = EncryptionHelper.EncryptString(value);
        }

        private string _lengthOfStay;
        [Required(ErrorMessage = "Length of Stay is required")]
        public string LengthOfStay
        {
            get => EncryptionHelper.DecryptString(_lengthOfStay);
            set => _lengthOfStay = EncryptionHelper.EncryptString(value);
        }

        private string _height;
        [Required(ErrorMessage = "Height is required")]
        public string Height
        {
            get => EncryptionHelper.DecryptString(_height);
            set => _height = EncryptionHelper.EncryptString(value);
        }

        private string _weight;
        [Required(ErrorMessage = "Weight is required")]
        public string Weight
        {
            get => EncryptionHelper.DecryptString(_weight);
            set => _weight = EncryptionHelper.EncryptString(value);
        }

        private string _gender;
        [Required(ErrorMessage = "Gender is required")]
        public string Gender
        {
            get => EncryptionHelper.DecryptString(_gender);
            set => _gender = EncryptionHelper.EncryptString(value);
        }

        private string _civilStatus;
        [Required(ErrorMessage = "Civil Status is required")]
        public string CivilStatus
        {
            get => EncryptionHelper.DecryptString(_civilStatus);
            set => _civilStatus = EncryptionHelper.EncryptString(value);
        }

        private string _contactNumber;
        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNumber
        {
            get => EncryptionHelper.DecryptString(_contactNumber);
            set => _contactNumber = EncryptionHelper.EncryptString(value);
        }

        private string _dateOfBirth;
        [Required(ErrorMessage = "Date of Birth is required")]
        public string DateOfBirth
        {
            get => EncryptionHelper.DecryptString(_dateOfBirth);
            set => _dateOfBirth = EncryptionHelper.EncryptString(value);
        }

        private string _placeOfBirth;
        [Required(ErrorMessage = "Place of Birth is required")]
        public string PlaceOfBirth
        {
            get => EncryptionHelper.DecryptString(_placeOfBirth);
            set => _placeOfBirth = EncryptionHelper.EncryptString(value);
        }

        private string _religion;
        [Required(ErrorMessage = "Religion is required")]
        public string Religion
        {
            get => EncryptionHelper.DecryptString(_religion);
            set => _religion = EncryptionHelper.EncryptString(value);
        }

        private string _email;
        [Required(ErrorMessage = "Email is required")]
        public string Email
        {
            get => EncryptionHelper.DecryptString(_email);
            set => _email = EncryptionHelper.EncryptString(value);
        }

        private string _registered;
        [Required(ErrorMessage = "Registered is required")]
        public string Registered
        {
            get => EncryptionHelper.DecryptString(_registered);
            set => _registered = EncryptionHelper.EncryptString(value);
        }

        private string _precintNumber;
        [Required(ErrorMessage = "Precint Number is required")]
        public string PrecintNumber
        {
            get => EncryptionHelper.DecryptString(_precintNumber);
            set => _precintNumber = EncryptionHelper.EncryptString(value);
        }

        private string _remarks;
        public string? Remarks
        {
            get => EncryptionHelper.DecryptString(_remarks);
            set => _remarks = EncryptionHelper.EncryptString(value);
        }

        public bool SeniorCitizen { get; set; } = false;
        public bool StreetSweeper { get; set; } = false;
        public bool ActiveResident { get; set; } = false;
        public bool TakingMeds { get; set; } = false;

        public ICollection<MedsModel> Meds { get; set; }
        public bool PetOwner { get; set; } = false;
        public ICollection<PetModel> Pets { get; set; }

        public ICollection<EmployeeModel> Employee { get; set; }



        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public string? UserId { get; set; }

        public ResidentModel()
        {
            Pets = [];
            Meds = [];
            Employee = [];
            // Initialize other collections if needed
        }
    }
}
