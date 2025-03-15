namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanDetailsListModel : BaseListModel
    {
        public List<GymWorkoutPlanDetailsModel> GymWorkoutPlanDetailsList { get; set; }
        public GymWorkoutPlanDetailsListModel()
        {
            GymWorkoutPlanDetailsList = new List<GymWorkoutPlanDetailsModel>();
        }

    }
}



