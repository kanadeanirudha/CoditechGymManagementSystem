using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberMembershipPlanListViewModel : BaseViewModel
    {
        public List<GymMemberMembershipPlanViewModel> GymMemberMembershipPlanList { get; set; }
        public GymMemberMembershipPlanListViewModel()
        {
            GymMemberMembershipPlanList = new List<GymMemberMembershipPlanViewModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
    }
}
