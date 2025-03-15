using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanDetailsListViewModel : BaseViewModel
    {
        public List<GymWorkoutPlanDetailsViewModel> GymWorkoutPlanDetailsList { get; set; }
        public GymWorkoutPlanDetailsListViewModel()
        {
            GymWorkoutPlanDetailsList = new List<GymWorkoutPlanDetailsViewModel>();
        }      
    }
}
