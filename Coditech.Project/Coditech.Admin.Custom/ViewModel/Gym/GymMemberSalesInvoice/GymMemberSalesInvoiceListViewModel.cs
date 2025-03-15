using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberSalesInvoiceListViewModel : BaseViewModel
    {
        public List<GymMemberSalesInvoiceViewModel> GymMemberSalesInvoiceList { get; set; }
        public GymMemberSalesInvoiceListViewModel()
        {
            GymMemberSalesInvoiceList = new List<GymMemberSalesInvoiceViewModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedCentreCode { get; set; }
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
        public string salesInvoiceMasterId { get; set; }

    }
}
