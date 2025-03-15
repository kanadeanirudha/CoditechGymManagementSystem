using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymMembershipPlanListViewModel : BaseViewModel
    {
        public List<GymMembershipPlanViewModel> GymMembershipPlanList { get; set; }
        public GymMembershipPlanListViewModel()
        {
            GymMembershipPlanList = new List<GymMembershipPlanViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
    }
}
