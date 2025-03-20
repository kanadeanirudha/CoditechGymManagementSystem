using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanUserModel : BaseModel
    {
        public long GymWorkoutPlanUserId { get; set; }
        public int GymMemberDetailId { get; set; }
        public long GymWorkoutPlanId { get; set; }
        public bool IsAssociated { get; set; }
        public bool IsPlanActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string ImagePath { get; set; }
        public string WorkoutPlanName { get; set; }
        public bool IsGymWorkoutPlanActive { get; set; }
    }
}
