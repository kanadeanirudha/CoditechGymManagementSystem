using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanUserListViewModel : BaseViewModel
    {
        public List<GymWorkoutPlanUserViewModel> GymWorkoutPlanUserList { get; set; }

        public GymWorkoutPlanUserListViewModel()
        {
            GymWorkoutPlanUserList = new List<GymWorkoutPlanUserViewModel>();
        }
        public int GymMemberDetailId { get; set; }
        public long GymWorkoutPlanId { get; set; }
        public string WorkoutPlanName { get; set; }
        public string SelectedParameter1 { get; set; }
    
    }
}
