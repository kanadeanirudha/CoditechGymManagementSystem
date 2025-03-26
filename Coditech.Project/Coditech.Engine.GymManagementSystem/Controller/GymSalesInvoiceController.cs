using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Controllers
{
    public class GymSalesInvoiceController : BaseController
    {
        private readonly IGymSalesInvoiceService _gymSalesInvoiceService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymSalesInvoiceController(ICoditechLogging coditechLogging, IGymSalesInvoiceService gymSalesInvoiceService)
        {
            _gymSalesInvoiceService = gymSalesInvoiceService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/GymSalesInvoice/GymMemberServiceSalesInvoiceList")]
        [Produces(typeof(GymMemberSalesInvoiceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GymServiceSalesInvoiceList(int adminRoleMasterId, string selectedCentreCode, DateTime? fromDate, DateTime? toDate, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberSalesInvoiceListModel list = _gymSalesInvoiceService.GymMemberServiceSalesInvoiceList(adminRoleMasterId,selectedCentreCode, fromDate, toDate, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberSalesInvoiceListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberSalesInvoiceListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberSalesInvoiceListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }


        [Route("/GymSalesInvoice/GetSalesInvoiceDetails")]
        [HttpGet]
        [Produces(typeof(SalesInvoicePrintResponse))]
        public virtual IActionResult GetSalesInvoiceDetails(long salesInvoiceMasterId)
        {
            try
            {
                SalesInvoicePrintModel salesInvoicePrintModel = _gymSalesInvoiceService.GetSalesInvoiceDetails(salesInvoiceMasterId);
                return IsNotNull(salesInvoicePrintModel) ? CreateOKResponse(new SalesInvoicePrintResponse { SalesInvoicePrintModel = salesInvoicePrintModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.SalesAndPurchase.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new SalesInvoicePrintResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.SalesAndPurchase.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new SalesInvoicePrintResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}