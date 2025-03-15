using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymBodyMeasurementTypeViewModel : BaseViewModel
    {
        [Required]
        public short GymBodyMeasurementTypeId { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Body Measurement Type")]
        public string BodyMeasurementType { get; set; }

        public string MeasurementUnitDisplayName { get; set; }

        [Required]
        [Display(Name = "Measurement Unit")]
        public short GeneralMeasurementUnitMasterId { get; set; }

        [Display(Name = "Display Order")]
        public short DisplayOrder { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
