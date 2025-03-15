using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Resources;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberSalesInvoiceViewModel : BaseViewModel
    {
        public GymMemberSalesInvoiceModel GymMemberSalesInvoice;
        [Display(Name = "Sales Invoice Master ID")]
        public long SalesInvoiceMasterId { get; set; }

        [Display(Name = "Person ID")]
        public long PersonId { get; set; }

        [Display(Name = "Gym Member Detail ID")]
        public int GymMemberDetailId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [Display(Name = "Person Code")]
        public string PersonCode { get; set; }

        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; } = "0";

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Bill Amount")]
        public decimal BillAmount { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Total Amount")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Plan Type")]
        public string PlanType { get; set; }

        [Display(Name = "Membership Plan Name")]
        public string MembershipPlanName { get; set; }

        [Display(Name = "Plan Duration Type")]
        public string PlanDurationType { get; set; }

        [Display(Name = "Plan Start Date")]
        public DateTime PlanStartDate { get; set; }

        [Display(Name = "Plan Duration Expiration Date")]
        public DateTime PlanDurationExpirationDate { get; set; }

        [Display(Name = "Session")]
        public short PlanDurationInSession { get; set; }

        [Display(Name = "Remaining Session Count")]
        public short RemainingSessionCount { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }

        [Display(Name = "Payment Received By")]
        public string PaymentReceivedBy { get; set; }
        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string SelectedCentreCode { get; set; }
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }



    }
}
