using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanDetailsViewModel : BaseViewModel
    {
        public GymWorkoutPlanModel GymWorkoutPlanModel { get; set; }
        public List<GymWorkoutPlanSetModel> GymWorkoutPlanSetList { get; set; }
        public long GymWorkoutPlanDetailId { get; set; }
        public long GymWorkoutPlanId { get; set; }
        [Required]
        [Display(Name = "Workout Name")]
        public string WorkoutName { get; set; }
        public short WorkoutWeek { get; set; }
        public byte WorkoutDay { get; set; }
        public string GymWorkoutPlanData { get; set; }
    }
}
