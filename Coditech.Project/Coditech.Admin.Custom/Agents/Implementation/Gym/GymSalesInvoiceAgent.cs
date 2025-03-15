using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class GymSalesInvoiceAgent : BaseAgent, IGymSalesInvoiceAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymSalesInvoiceClient _gymSalesInvoiceClient;
        #endregion

        #region Public Constructor
        public GymSalesInvoiceAgent(ICoditechLogging coditechLogging, IGymSalesInvoiceClient gymSalesInvoiceClient)
        {
            _coditechLogging = coditechLogging;
            _gymSalesInvoiceClient = GetClient<IGymSalesInvoiceClient>(gymSalesInvoiceClient);
        }
        #endregion

        #region Public Methods
        public virtual GymMemberSalesInvoiceListViewModel GymMemberServiceSalesInvoiceList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("InvoiceNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }
            dataTableModel.SelectedParameter1 = string.IsNullOrEmpty(dataTableModel.SelectedParameter1) ? Convert.ToDateTime(DateTime.Now.AddMonths(-1)).ToShortDateString() : dataTableModel.SelectedParameter1;
            dataTableModel.SelectedParameter2 = string.IsNullOrEmpty(dataTableModel.SelectedParameter2) ? Convert.ToDateTime(DateTime.Now).ToShortDateString() : dataTableModel.SelectedParameter2;
            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);
           
            GymMemberSalesInvoiceListResponse response = _gymSalesInvoiceClient.GymMemberServiceSalesInvoiceList(dataTableModel.SelectedCentreCode, Convert.ToDateTime(dataTableModel.SelectedParameter1), Convert.ToDateTime(dataTableModel.SelectedParameter2), null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMemberSalesInvoiceListModel gymMemberSalesInvoiceList = new GymMemberSalesInvoiceListModel { GymMemberSalesInvoiceList = response?.GymMemberSalesInvoiceList };
            GymMemberSalesInvoiceListViewModel listViewModel = new GymMemberSalesInvoiceListViewModel();
            listViewModel.GymMemberSalesInvoiceList = gymMemberSalesInvoiceList?.GymMemberSalesInvoiceList?.ToViewModel<GymMemberSalesInvoiceViewModel>().ToList();
            listViewModel.SelectedParameter1 = dataTableModel.SelectedParameter1;
            listViewModel.SelectedParameter2 = dataTableModel.SelectedParameter2;
            listViewModel.SelectedCentreCode = dataTableModel.SelectedCentreCode;
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMemberSalesInvoiceList.Count, BindColumns());
            return listViewModel;
        }

        //Get Sales Invoice Details
        public virtual SalesInvoicePrintModel GetSalesInvoiceDetails(long salesInvoiceMasterId)
        {
            SalesInvoicePrintResponse response = _gymSalesInvoiceClient.GetSalesInvoiceDetails(salesInvoiceMasterId);
            SalesInvoicePrintModel salesInvoicePrintModel = response?.SalesInvoicePrintModel;
            return salesInvoicePrintModel;
        }

        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Invoice Number",
                ColumnCode = "InvoiceNumber",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Gym Member",
                ColumnCode = "FirstName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Transaction Date",
                ColumnCode = "TransactionDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Type",
                ColumnCode = "PlanTypeEnumId",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Name",
                ColumnCode = "MembershipPlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Paid Amount",
                ColumnCode = "BillAmount",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Payment Mode",
                ColumnCode = "PaymentType",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Payment Received By",
                ColumnCode = "PaymentReceivedBy",
            });
            return datatableColumnList;
        }
        #endregion
    }
}
