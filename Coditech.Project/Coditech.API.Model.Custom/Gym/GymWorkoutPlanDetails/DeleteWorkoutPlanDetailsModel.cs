namespace Coditech.Common.API.Model
{
    public class DeleteWorkoutPlanDetailsModel
    {
        /// <summary>
        /// This helps to pass in query parameter (Comma seperated string)
        /// </summary>  
        public string GymWorkoutplanDeleteType { get; set; }
        public long GymWorkoutPlanId { get; set; }
        public long GymWorkoutPlandetailId { get; set; }
        public short WorkoutWeekNumber { get; set; }     
        public byte WorkoutDayNumber { get; set; } 
        public long GymWorkoutSetId { get; set; }
    }
}
