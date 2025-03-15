namespace Coditech.Common.API.Model
{
    public class GymMemberSalesInvoiceListModel : BaseListModel
    {
        public List<GymMemberSalesInvoiceModel> GymMemberSalesInvoiceList { get; set; }
        public GymMemberSalesInvoiceListModel()
        {
            GymMemberSalesInvoiceList = new List<GymMemberSalesInvoiceModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
