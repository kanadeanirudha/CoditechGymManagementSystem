using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberSalesInvoiceReportViewModel : BaseViewModel
    {
        [Required]
        public string SelectedCentreCode { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Required]
        public string ReportType { get; set; }

        public bool IsRecordFound { get; set; } = true;
    }
}
