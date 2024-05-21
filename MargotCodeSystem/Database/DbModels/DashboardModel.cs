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

        public string fullName { get; set; }

        public bool seniorCitizen { get; set; }

        public bool medicationUser { get; set; }

        public bool streetSweeper { get; set; }

        public bool petOwner { get; set; }

        public bool activeResident { get; set; }
        [Required]

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }

        public int ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ResidentModel ResidentModel { get; set; }

        public int? HouseOccupantId { get; set; }
        [ForeignKey("HouseOccupantId")]
        public HouseOccupantModel HouseOccupantModel { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
