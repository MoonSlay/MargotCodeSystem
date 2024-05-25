using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Database.DbModels
{
    public class HouseOccupantModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? HouseName { get; set; }
        public string? fullName { get; set; }

        public string? Position { get; set; }

        public int? Age { get; set; }

        public string? BirthDate { get; set; }

        public string? CivilStatus { get; set; }

        public string? SourceIncome { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public string? UserId { get; set; }
        public int? ResidentId { get; set; }

    }
}
