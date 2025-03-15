using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanSetListViewModel : BaseViewModel
    {
        public List<GymWorkoutPlanSetViewModel> GymWorkoutPlanSetList { get; set; }
        public GymWorkoutPlanSetListViewModel()
        {
            GymWorkoutPlanSetList = new List<GymWorkoutPlanSetViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
    }
}
