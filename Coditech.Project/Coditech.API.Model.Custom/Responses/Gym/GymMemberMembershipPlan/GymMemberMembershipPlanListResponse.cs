namespace Coditech.Common.API.Model.Response
{
    public class GymMemberMembershipPlanListResponse : BaseListResponse
    {
        public List<GymMemberMembershipPlanModel> GymMemberMembershipPlanList { get; set; }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
