using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

using Microsoft.AspNetCore.Mvc;

namespace Coditech.Admin.Controllers
{
	public class GymMemberSalesInvoiceController : BaseController
    {
        private readonly IGymSalesInvoiceAgent _gymSaleInvoiceAgent;

        public GymMemberSalesInvoiceController(IGymSalesInvoiceAgent gymSaleInvoiceAgent)
        {
            _gymSaleInvoiceAgent = gymSaleInvoiceAgent;
        }

        #region GymMemberSaleInvoice
        public ActionResult SaleInvoiceList(DataTableViewModel dataTableModel)
        {
            GymMemberSalesInvoiceListViewModel list = new GymMemberSalesInvoiceListViewModel();
            GetListOnlyIfSingleCentre(dataTableModel);
            if (!string.IsNullOrEmpty(dataTableModel.SelectedCentreCode))
            {
                list = _gymSaleInvoiceAgent.GymMemberServiceSalesInvoiceList(dataTableModel);
                list.FromDate = Convert.ToDateTime(dataTableModel.SelectedParameter1);
                list.ToDate = Convert.ToDateTime(dataTableModel.SelectedParameter2);
            }
            else
            {
                list.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
                list.ToDate = Convert.ToDateTime(DateTime.Now);
            }
            if (AjaxHelper.IsAjaxRequest)
            {
                return PartialView("~/Views/Gym/GymSalesInvoice/_SaleInvoiceList.cshtml", list);
            }

            return View($"~/Views/Gym/GymSalesInvoice/SaleInvoiceList.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult GetSalesInvoiceDetails(long salesInvoiceMasterId ,string SelectedParameter1, string SelectedParameter2,string CentreCode)
        {
            SalesInvoicePrintModel salesInvoicePrintModel = _gymSaleInvoiceAgent.GetSalesInvoiceDetails(salesInvoiceMasterId);
            salesInvoicePrintModel.CentreCode = CentreCode;
            salesInvoicePrintModel.FromDate = SelectedParameter1;
            salesInvoicePrintModel.ToDate = SelectedParameter2;
			return View($"~/Views/Gym/GymSalesInvoice/SalesInvoicePrint.cshtml", salesInvoicePrintModel);
		}
        #endregion
    }
}
