namespace Coditech.API.Data
{
    public partial class GymBodyMeasurementType
    {
        public short GymBodyMeasurementTypeId { get; set; }
        public string BodyMeasurementType { get; set; }
        public short GeneralMeasurementUnitMasterId { get; set; }
        public short DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

