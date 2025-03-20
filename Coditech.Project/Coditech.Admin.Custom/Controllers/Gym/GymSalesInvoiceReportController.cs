using AspNetCore.Reporting;
using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Text;

namespace Coditech.Admin.Controllers
{
    public class GymSalesInvoiceReportController : BaseController
    {
        private IWebHostEnvironment Environment;
        private readonly IGymSalesInvoiceAgent _gymSaleInvoiceAgent;

        public GymSalesInvoiceReportController(IWebHostEnvironment _environment, IGymSalesInvoiceAgent gymSaleInvoiceAgent)
        {
            this.Environment = _environment;
            _gymSaleInvoiceAgent = gymSaleInvoiceAgent;
        }

        [HttpGet]
        public virtual IActionResult SaleInvoiceReport()
        {
            GymMemberSalesInvoiceReportViewModel model = new GymMemberSalesInvoiceReportViewModel();
            model.FromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            model.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return View("~/Views/Gym/Reports/SaleInvoiceReport.cshtml", model);
        }

        [HttpPost]
        public IActionResult SaleInvoiceReport(GymMemberSalesInvoiceReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                DataTableViewModel dataTableModel = new DataTableViewModel()
                {
                    SelectedCentreCode = model.SelectedCentreCode,
                    SelectedParameter1 = Convert.ToString(model.FromDate),
                    SelectedParameter2 = Convert.ToString(model.ToDate),
                    PageSize = int.MaxValue
                };
                GymMemberSalesInvoiceListViewModel list = _gymSaleInvoiceAgent.GymMemberServiceSalesInvoiceList(dataTableModel);
                if (list?.GymMemberSalesInvoiceList?.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Clear();
                    dataTable.Columns.Add("InvoiceNumber", typeof(string));
                    dataTable.Columns.Add("TransactionDate", typeof(string));
                    dataTable.Columns.Add("MemberName", typeof(string));
                    dataTable.Columns.Add("MobileNumber", typeof(string));
                    dataTable.Columns.Add("PlanType", typeof(string));
                    dataTable.Columns.Add("MembershipPlanName", typeof(string));
                    dataTable.Columns.Add("NetAmount", typeof(decimal));
                    dataTable.Columns.Add("TaxAmount", typeof(decimal));
                    dataTable.Columns.Add("DiscountAmount", typeof(decimal));
                    dataTable.Columns.Add("BillAmount", typeof(decimal));
                    dataTable.Columns.Add("TotalAmount", typeof(decimal));
                    dataTable.Columns.Add("PaymentType", typeof(string));
                    dataTable.Columns.Add("PaymentReceivedBy", typeof(string));

                    foreach (var item in list.GymMemberSalesInvoiceList)
                    {
                        dataTable.Rows.Add(item.InvoiceNumber, item.TransactionDate, $"{item.FirstName} {item.LastName}", item.MobileNumber, item.PlanType, item.MembershipPlanName,
                                            item.NetAmount, item.TaxAmount, item.DiscountAmount, item.BillAmount, item.TotalAmount,
                                            item.PaymentType, item.PaymentReceivedBy
                                          );
                    }

                    Dictionary<string, string> reportParameters = new Dictionary<string, string>();

                    string reportName = "SalesInvoice";

                    Stream stream = null;

                    if (model.ReportType == "xls")
                    {
                        stream = GetReport(this.Environment, "Gym", "GymSalesInvoice", dataTable, "DataSet1", reportParameters, RenderType.Excel);
                        return File(stream, "application/xls", $"{reportName}.xls");
                    }
                    else
                    {
                        stream = GetReport(this.Environment, "Gym", "GymSalesInvoice", dataTable, "DataSet1", reportParameters, RenderType.Pdf);
                        return File(stream, "application/pdf", $"{reportName}.pdf");
                    }
                }
                else
                {
                    model.IsRecordFound = false;
                }
            }
            return View("~/Views/Gym/Reports/SaleInvoiceReport.cshtml", model);
        }
    }
}