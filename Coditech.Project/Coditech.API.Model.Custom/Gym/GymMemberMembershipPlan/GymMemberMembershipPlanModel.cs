namespace Coditech.Common.API.Model
{
    public class GymMemberMembershipPlanModel : BaseModel
    {
        public GymMembershipPlanModel GymMembershipPlan;
        public long GymMemberMembershipPlanId { get; set; }
        public int GymMemberDetailId { get; set; }
        public long PersonId { get; set; }
        public int GymMembershipPlanId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDurationExpirationDate { get; set; }
        public short RemainingSessionCount { get; set; }
        public decimal PlanAmount { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal BillAmount { get; set; }
        public int PaymentTypeEnumId { get; set; }
        public string TransactionReference { get; set; }
        public string Remark { get; set; }
        public bool IsTransfered { get; set; }
        public int? TransferedGymMemberDetailId { get; set; }
        public long SalesInvoiceMasterId { get; set; }
        public bool IsExpired { get; set; }
        public string PlanType { get; set; }
        public string MembershipPlanName { get; set; }
        public string PlanDurationType { get; set; }
        public short PlanDurationInSession { get; set; }
    }
}
