using MargotCodeSystem.Database.DbModels.ResidentModels;

namespace MargotCodeSystem.Database.DbModels
{
    public class ResidentViewModel
    {
        public ResidentModel Resident { get; set; }

        public List<HouseOccupantGroupModel> HouseoccupantGroup { get; set; }

        public List<PetModel> Pets { get; set; }

        public List<MedsModel> Meds { get; set; }
    }
}
