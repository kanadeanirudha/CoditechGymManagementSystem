using Coditech.Common.API.Model;
using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberMembershipPlanViewModel : BaseViewModel
    {
        public GymMembershipPlanModel GymMembershipPlan;
        public long GymMemberMembershipPlanId { get; set; }
        public int GymMemberDetailId { get; set; }

        [Display(Name = "Membership Plan")]
        public int GymMembershipPlanId { get; set; }
       
        [Required]
        [Display(Name = "Plan Start Date")]
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDurationExpirationDate { get; set; }
        public short RemainingSessionCount { get; set; }

        [Required]
        [Display(Name = "Membership Plan Amount")]
        public decimal PlanAmount { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; } = 0;

        [Display(Name = "Mode Of Transaction")]
        public int PaymentTypeEnumId { get; set; }

        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; }
        public string Remark { get; set; }
        public bool IsTransfered { get; set; }
        public int? TransferedGymMemberDetailId { get; set; }
        public bool IsExpired { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CentreCode { get; set; }
		public string PlanType { get; set; }
		public string MembershipPlanName { get; set; }
		public string PlanDurationType { get; set; }
        public short PlanDurationInSession { get; set; }
    }
}
