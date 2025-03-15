namespace Coditech.Common.API.Model.Response
{
    public class GymWorkoutPlanUserListResponse : BaseListResponse
    {
        public List<GymWorkoutPlanUserModel> GymWorkoutPlanUserList { get; set; }
        public string GymWorkoutPlanId { get; set; }
        public string WorkoutPlanName { get; set; }
    }
}

