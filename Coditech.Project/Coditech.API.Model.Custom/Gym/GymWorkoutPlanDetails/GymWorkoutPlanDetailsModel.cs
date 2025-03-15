namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanDetailsModel : BaseModel
    {
        public GymWorkoutPlanDetailsModel()
        {
        }
        public List<GymWorkoutPlanSetModel> GymWorkoutPlanSetList { get; set; }
        public long GymWorkoutPlanDetailId { get; set; }
        public long GymWorkoutPlanId { get; set; }
        public string WorkoutName { get; set; }
        public short WorkoutWeek { get; set; }
        public byte WorkoutDay { get; set; }
    }
}
