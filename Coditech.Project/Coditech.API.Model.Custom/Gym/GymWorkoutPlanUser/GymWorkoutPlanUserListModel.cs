namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanUserListModel : BaseListModel
    {
        public List<GymWorkoutPlanUserModel> GymWorkoutPlanUserList { get; set; }
        public GymWorkoutPlanUserListModel()
        {
            GymWorkoutPlanUserList = new List<GymWorkoutPlanUserModel>();
        }
        public string WorkoutPlanName { get; set; }
        public long GymWorkoutPlanId { get; set; }
    }
}

