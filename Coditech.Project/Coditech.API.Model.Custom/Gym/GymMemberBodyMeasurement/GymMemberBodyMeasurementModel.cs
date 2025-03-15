using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymMemberBodyMeasurementModel : BaseModel
    {
        [Required]
        public long GymMemberBodyMeasurementId { get; set; }

        [Required]
        public int GymMemberDetailId { get; set; } 

        [Required]
        public short GymBodyMeasurementTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BodyMeasurementValue { get; set; }
        public string BodyMeasurementType { get; set; }
        public string MeasurementUnitShortCode { get; set; }
        public string MeasurementUnitDisplayName { get; set; }
        public DateTime BodyMeasurementDate { get; set; }
        public List<GymMemberBodyMeasurementValueModel> GymMemberBodyMeasurementValueList { get; set; }
        public GymMemberBodyMeasurementModel()
        {
            GymMemberBodyMeasurementValueList = new List<GymMemberBodyMeasurementValueModel>();
        }
    }
}
