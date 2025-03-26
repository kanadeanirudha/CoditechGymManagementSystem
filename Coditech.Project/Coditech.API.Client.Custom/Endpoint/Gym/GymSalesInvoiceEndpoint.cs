using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymSalesInvoiceEndpoint : BaseEndpoint
    {
        public string GymMemberServiceSalesInvoiceListAsync(int adminRoleMasterId, string selectedCentreCode, DateTime fromDate, DateTime toDate, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymSalesInvoice/GymMemberServiceSalesInvoiceList?adminRoleMasterId={adminRoleMasterId}&selectedCentreCode={selectedCentreCode}&fromDate={fromDate}&toDate={toDate}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetSalesInvoiceDetailsAsync(long salesInvoiceMasterId) =>
           $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymSalesInvoice/GetSalesInvoiceDetails?salesInvoiceMasterId={salesInvoiceMasterId}";
    }
}
