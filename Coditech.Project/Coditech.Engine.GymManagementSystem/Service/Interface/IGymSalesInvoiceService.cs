using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymSalesInvoiceService
    {
        GymMemberSalesInvoiceListModel GymMemberServiceSalesInvoiceList(string selectedCentreCode, DateTime? fromDate, DateTime? toDate, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        SalesInvoicePrintModel GetSalesInvoiceDetails(long salesInvoiceMasterId);
    }
}
