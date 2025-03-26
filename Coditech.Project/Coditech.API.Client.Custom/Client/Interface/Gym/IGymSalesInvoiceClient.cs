using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IGymSalesInvoiceClient : IBaseClient
    {
        /// <summary>
        /// Get Gym Service Sales Invoice List
        /// </summary>
        /// <returns>GymMemberSalesInvoiceListResponse</returns>
        GymMemberSalesInvoiceListResponse GymMemberServiceSalesInvoiceList(int adminRoleMasterId, string selectedCentreCode, DateTime fromDate, DateTime toDate, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Get Sales Invoice Details by salesInvoiceMasterId.
        /// </summary>
        /// <param name="gymMembershipPlanId">salesInvoiceMasterId</param>
        /// <returns>Returns SalesInvoicePrintResponse.</returns>
        SalesInvoicePrintResponse GetSalesInvoiceDetails(long salesInvoiceMasterId);
    }
}
