using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanListViewModel : BaseViewModel
    {
        public List<GymWorkoutPlanViewModel> GymWorkoutPlanList { get; set; }
        public GymWorkoutPlanListViewModel()
        {
            GymWorkoutPlanList = new List<GymWorkoutPlanViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
       
    }
}
