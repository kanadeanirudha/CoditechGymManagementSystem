using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymWorkoutPlanDetails
    {
        [Key]
        public long GymWorkoutPlanDetailId { get; set; }        
        public long GymWorkoutPlanId { get; set; }
        public string WorkoutName { get; set; }
        public short WorkoutWeek { get; set; }
        public byte WorkoutDay { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

