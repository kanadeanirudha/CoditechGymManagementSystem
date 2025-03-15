using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymWorkoutPlanUser
    {
        [Key]
        public long GymWorkoutPlanUserId { get; set; }
        public int GymMemberDetailId { get; set; }
        public long GymWorkoutPlanId { get; set; }       
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

