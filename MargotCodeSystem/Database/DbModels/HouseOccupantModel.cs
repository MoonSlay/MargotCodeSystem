using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MargotCodeSystem.Utils;

namespace MargotCodeSystem.Database.DbModels
{
    public class HouseOccupantModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string? _houseName;
        public string? HouseName
        {
            get => EncryptionHelper.DecryptString(_houseName);
            set => _houseName = EncryptionHelper.EncryptString(value);
        }

        private string? _fullName;
        public string? FullName
        {
            get => EncryptionHelper.DecryptString(_fullName);
            set => _fullName = EncryptionHelper.EncryptString(value);
        }

        private string? _position;
        public string? Position
        {
            get => EncryptionHelper.DecryptString(_position);
            set => _position = EncryptionHelper.EncryptString(value);
        }

        private string? _age;
        public string? Age
        {
            get => EncryptionHelper.DecryptString(_age);
            set => _age = EncryptionHelper.EncryptString(value);
        }


        private string? _birthDate;
        public string? BirthDate
        {
            get => EncryptionHelper.DecryptString(_birthDate);
            set => _birthDate = EncryptionHelper.EncryptString(value);
        }

        private string? _civilStatus;
        public string? CivilStatus
        {
            get => EncryptionHelper.DecryptString(_civilStatus);
            set => _civilStatus = EncryptionHelper.EncryptString(value);
        }

        private string? _sourceIncome;
        public string? SourceIncome
        {
            get => EncryptionHelper.DecryptString(_sourceIncome);
            set => _sourceIncome = EncryptionHelper.EncryptString(value);
        }


        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public string? UserId { get; set; }
        public int? ResidentId { get; set; }

    }
}
