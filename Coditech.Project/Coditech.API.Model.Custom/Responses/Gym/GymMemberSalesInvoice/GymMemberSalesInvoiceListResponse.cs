namespace Coditech.Common.API.Model.Response
{
    public class GymMemberSalesInvoiceListResponse : BaseListResponse
    {
        public List<GymMemberSalesInvoiceModel> GymMemberSalesInvoiceList { get; set; }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
