using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymWorkoutPlanSetModel : BaseModel
    {
        public long GymWorkoutSetId { get; set; }
        public long GymWorkoutPlanDetailId { get; set; }
        [Required(ErrorMessage = "Please enter weight.")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Please enter the number of repetitions.")]
        public short Repetitions { get; set; }
        [Required(ErrorMessage = "Please enter duration in seconds.")]
        public short Duration { get; set; }
    }
}
