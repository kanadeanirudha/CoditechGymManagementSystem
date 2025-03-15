using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Resources;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanViewModel : BaseViewModel
    {
        public List<GymWorkoutPlanDetailsModel> GymWorkoutPlanDetailList { get; set; }
        public List<GymWorkoutPlanSetModel> GymWorkoutPlanSetList { get; set; }
        public long GymWorkoutPlanId { get; set; }

        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string CentreCode { get; set; }

        [Required]
        [Display(Name = "Workout Plan Name")]
        public string WorkoutPlanName { get; set; }

        [Required]
        [Display(Name = "Target")]
        public string Target { get; set; }

        [Required]
        [Display(Name = "Number Of Weeks")]
        public byte NumberOfWeeks { get; set; }

        [Required]
        [Display(Name = "Number Of Days Per Week")]
        public short NumberOfDaysPerWeek { get; set; }
 
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
    }
}
