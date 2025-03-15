using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Agents
{
    public interface IGymSalesInvoiceAgent
    {
        /// <summary>
        /// Get Gym Member Service Sales Invoice List
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberSalesInvoiceListViewModel</returns>
        GymMemberSalesInvoiceListViewModel GymMemberServiceSalesInvoiceList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Get Sales Invoice Details by SalesInvoicePrintld.
        /// </summary>
        /// <param name="personId">salesInvoiceMasterId</param>
        /// <returns>Returns SalesInvoicePrintModel.</returns>
        SalesInvoicePrintModel GetSalesInvoiceDetails(long salesInvoiceMasterId);
    }
}
