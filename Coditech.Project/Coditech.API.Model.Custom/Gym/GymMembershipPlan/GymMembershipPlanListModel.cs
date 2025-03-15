namespace Coditech.Common.API.Model
{
    public class GymMembershipPlanListModel : BaseListModel
    {
        public List<GymMembershipPlanModel> GymMembershipPlanList { get; set; }
        public GymMembershipPlanListModel()
        {
            GymMembershipPlanList = new List<GymMembershipPlanModel>();
        }
    }
}
