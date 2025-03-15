using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymMembershipPlanModel : BaseModel
    {
        public int GymMembershipPlanId { get; set; }

        [MaxLength(100)]
        public string MembershipPlanName { get; set; }
        public string CentreCode { get; set; }
        public decimal MaxCost { get; set; }
        public decimal MinCost { get; set; }
        public int PlanTypeEnumId { get; set; }
        public string PlanType { get; set; }
        public int PlanDurationTypeEnumId { get; set; }
        public string PlanDurationType { get; set; }
        [Required]
        public byte PlanDurationInMonth { get; set; }
        [Required]
        public short PlanDurationInDays { get; set; }
        public short? PlanDurationInSession { get; set; }
        public bool IsRenewable { get; set; }
        public bool IsTimebaseBiometricAccess { get; set; }
        public bool IsTaxExclusive { get; set; }
        [Required]
        public TimeSpan? FromTime { get; set; }
        [Required]
        public TimeSpan? ToTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsEditable { get; set; }
		public List<string> SelectedGeneralServicesIds { get; set; }

	}
}
