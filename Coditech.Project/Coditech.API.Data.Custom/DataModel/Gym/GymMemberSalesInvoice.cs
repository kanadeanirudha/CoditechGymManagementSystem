using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymMemberSalesInvoice
    {
        [Key]
        public long SalesInvoiceMasterId { get; set; }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string PersonCode { get; set; }
        public string InvoiceNumber { get; set; } = "0";
        public DateTime TransactionDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BillAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string PlanType { get; set; }
        public string MembershipPlanName { get; set; }
        public string PlanDurationType { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanDurationExpirationDate { get; set; }
        public short PlanDurationInSession { get; set; }
        public short RemainingSessionCount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentReceivedBy { get; set; }
        public string SelectedCentreCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}

