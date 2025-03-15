using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymMemberMembershipPlan
    {
        [Key]
        public long GymMemberMembershipPlanId { get; set; }
        public int GymMembershipPlanId { get; set; }
        public int GymMemberDetailId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDurationExpirationDate { get; set; }
        public short RemainingSessionCount { get; set; }
        public decimal PlanAmount { get; set; }
        public bool IsExpired { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public int PaymentTypeEnumId { get; set; }
        public string TransactionReference { get; set; }
        public string Remark { get; set; }
        public bool IsTransfered { get; set; }
        public Nullable<int> TransferedGymMemberDetailId { get; set; }
        public long SalesInvoiceMasterId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

