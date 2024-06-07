using MargotCodeSystem.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MargotCodeSystem.Database.DbModels.ResidentModels
{
    public class PetModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string? _name;
        public string? Name
        {
            get => EncryptionHelper.DecryptString(_name);
            set => _name = EncryptionHelper.EncryptString(value);
        }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public int ResidentId { get; set; }
    }
}
