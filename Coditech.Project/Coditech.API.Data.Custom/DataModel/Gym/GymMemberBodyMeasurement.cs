using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymMemberBodyMeasurement
    {

        [Key]
        public long GymMemberBodyMeasurementId { get; set; }
        public int GymMemberDetailId { get; set; }
        public short GymBodyMeasurementTypeId { get; set; }
        public string BodyMeasurementValue { get; set; }
        public DateTime BodyMeasurementDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

