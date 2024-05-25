using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MargotCodeSystem.Database.DbModels
{
    public class HouseOccupantGroupModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? HouseName { get; set; }
        public List<HouseOccupantModel> HouseOccupants { get; set; }

        public string? UserId { get; set; }
        public int? ResidentId { get; set; }
    }
}
