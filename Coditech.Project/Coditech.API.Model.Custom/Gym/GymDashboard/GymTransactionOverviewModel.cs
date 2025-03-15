namespace Coditech.Common.API.Model
{
    public class GymTransactionOverviewModel : BaseModel
    {
        public DateTime TransactionDate { get; set; }
        public string PaymentType { get; set; }
        public string MonthName { get; set; }
        public Decimal NetAmount { get; set; }
        public Decimal TaxAmount { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal BillAmount { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
