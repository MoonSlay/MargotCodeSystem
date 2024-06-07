using MargotCodeSystem.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Database.DbModels
{
    public class EmployeeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string? _duration;
        public string? EmployeeDuration
        {
            get => EncryptionHelper.DecryptString(_duration);
            set => _duration = EncryptionHelper.EncryptString(value);
        }
        private string? _name;
        public string? CompanyName
        {
            get => EncryptionHelper.DecryptString(_name);
            set => _name = EncryptionHelper.EncryptString(value);
        }
        private string? _employeer;
        public string? Employer
        {
            get => EncryptionHelper.DecryptString(_employeer);
            set => _employeer = EncryptionHelper.EncryptString(value);
        }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public int ResidentId { get; set; }
    }
}
