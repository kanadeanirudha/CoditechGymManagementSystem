namespace Coditech.Common.API.Model
{
    public class GymMemberMembershipPlanListModel : BaseListModel
    {
        public List<GymMemberMembershipPlanModel> GymMemberMembershipPlanList { get; set; }
        public GymMemberMembershipPlanListModel()
        {
            GymMemberMembershipPlanList = new List<GymMemberMembershipPlanModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
