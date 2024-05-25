using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MargotCodeSystem.Database.DbModels.ResidentModels
{
    public class MedsModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public int ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ResidentModel ResidentModel { get; set; }

    }
}
