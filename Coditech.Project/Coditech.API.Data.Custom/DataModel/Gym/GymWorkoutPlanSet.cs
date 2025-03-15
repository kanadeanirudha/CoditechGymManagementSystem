using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymWorkoutPlanSet
    {
        [Key]
        public long GymWorkoutSetId { get; set; }
        public long GymWorkoutPlanDetailId { get; set; }        
        public decimal Weight { get; set; }
        public short Repetitions { get; set; }
        public short Duration { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

