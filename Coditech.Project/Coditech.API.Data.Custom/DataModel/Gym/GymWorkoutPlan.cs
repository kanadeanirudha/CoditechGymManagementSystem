using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymWorkoutPlan
    {
        [Key]
        public long GymWorkoutPlanId { get; set; }
        public string CentreCode { get; set; }
        public string WorkoutPlanName { get; set; }
        public string Target { get; set; }
        public byte NumberOfWeeks { get; set; }
        public short NumberOfDaysPerWeek { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

