namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanModel : BaseModel
    {
        public List<GymWorkoutPlanDetailsModel> GymWorkoutPlanDetailList { get; set; }
        public long GymWorkoutPlanId { get; set; }        
        public string CentreCode { get; set; }
        public string WorkoutPlanName { get; set; } 
        public string Target { get; set; }
        public byte NumberOfWeeks { get; set; } 
        public short NumberOfDaysPerWeek { get; set; } 
        public bool IsActive { get; set; }
    }
}
