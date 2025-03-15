using Coditech.Common.API.Model;
using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymWorkoutPlanSetViewModel : BaseViewModel
    {
        public long GymWorkoutSetId { get; set; }
        public long GymWorkoutPlanDetailId { get; set; }
        public decimal Weight { get; set; }
        public short Repetitions { get; set; }
        public short Duration { get; set; }
        public string GymWorkoutPlanData { get; set; }
    }
}





