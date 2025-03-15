using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymBodyMeasurementTypeModel : BaseModel
    {
        [Required]
        public short GymBodyMeasurementTypeId { get; set; }

        [MaxLength(50)]
        [Required]
        public string BodyMeasurementType { get; set; }

        public string MeasurementUnitDisplayName { get; set; }

        [Required]
        public short GeneralMeasurementUnitMasterId { get; set; }

        public short DisplayOrder { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
