using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MargotCodeSystem.Utils;

namespace MargotCodeSystem.Database.DbModels
{
    public class HouseOccupantGroupModel
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
        public List<HouseOccupantModel> HouseOccupants { get; set; }

        public string? UserId { get; set; }
        public int? ResidentId { get; set; }
    }
}
