namespace Coditech.Common.API.Model.Response
{
    public class GymMemberBodyMeasurementListResponse : BaseListResponse
    {
        public List<GymMemberBodyMeasurementModel> GymMemberBodyMeasurementList { get; set; }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
