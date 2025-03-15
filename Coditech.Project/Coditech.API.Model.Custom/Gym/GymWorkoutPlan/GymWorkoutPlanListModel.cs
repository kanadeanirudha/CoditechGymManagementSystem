namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanListModel : BaseListModel
    {
        public List<GymWorkoutPlanModel> GymWorkoutPlanList { get; set; }
        public GymWorkoutPlanListModel()
        {
            GymWorkoutPlanList = new List<GymWorkoutPlanModel>();
        }
        public long GymWorkoutPlanId { get; set; }

    }
}


