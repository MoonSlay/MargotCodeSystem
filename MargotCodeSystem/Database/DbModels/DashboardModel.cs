using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MargotCodeSystem.Database.DbModels;
using System.ComponentModel;
using MargotCodeSystem.Models.Identity;

namespace MargotCodeSystem.Database.DbModels
{
    public class DashboardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? fullName { get; set; }

        public string? provincialAddress { get; set; }

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
