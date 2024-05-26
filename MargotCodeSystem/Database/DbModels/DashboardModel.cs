using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Utils;

namespace MargotCodeSystem.Database.DbModels
{
    public class DashboardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _fullName;
        private string _provincialAddress;

        public string? fullName
        {
            get => EncryptionHelper.DecryptString(_fullName);
            set => _fullName = EncryptionHelper.EncryptString(value);
        }

        public string? provincialAddress
        {
            get => EncryptionHelper.DecryptString(_provincialAddress);
            set => _provincialAddress = EncryptionHelper.EncryptString(value);
        }

        public bool seniorCitizen { get; set; } = false;

        public bool medicationUser { get; set; } = false;

        public bool streetSweeper { get; set; } = false;

        public bool petOwner { get; set; } = false;

        public bool activeResident { get; set; } = false;

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; } = false;

        public int? ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ResidentModel ResidentModel { get; set; }

        public string? UserId { get; set; }
    }
}