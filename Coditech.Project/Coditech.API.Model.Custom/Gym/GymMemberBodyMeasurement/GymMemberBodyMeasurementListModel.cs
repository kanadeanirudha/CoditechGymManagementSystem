namespace Coditech.Common.API.Model
{
    public class GymMemberBodyMeasurementListModel : BaseListModel
    {
        public List<GymMemberBodyMeasurementModel> GymMemberBodyMeasurementList { get; set; }
        public GymMemberBodyMeasurementListModel()
        {
            GymMemberBodyMeasurementList = new List<GymMemberBodyMeasurementModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
