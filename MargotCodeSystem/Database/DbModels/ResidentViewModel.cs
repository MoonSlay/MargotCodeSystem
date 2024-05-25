namespace MargotCodeSystem.Database.DbModels
{
    public class ResidentViewModel
    {
        public ResidentModel Resident { get; set; }

        public List<HouseOccupantGroupModel> HouseoccupantGroup { get; set; }
    }
}
