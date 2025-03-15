using Coditech.API.Data;
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
    public class GymMemberDetailsController : BaseController
    {
        private readonly IGymMemberDetailsService _generalGymMemberDetailsService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymMemberDetailsController(ICoditechLogging coditechLogging, IGymMemberDetailsService generalGymMemberDetailsService)
        {
            _generalGymMemberDetailsService = generalGymMemberDetailsService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/GymMemberDetails/GetGymMemberDetailsList")]
        [Produces(typeof(GymMemberDetailsListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGymMemberDetailsList(string selectedCentreCode, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberDetailsListModel list = _generalGymMemberDetailsService.GetGymMemberDetailsList(selectedCentreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberDetailsListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberDetailsListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberDetailsListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/GetGymMemberOtherDetails")]
        [HttpGet]
        [Produces(typeof(GymMemberDetailsResponse))]
        public virtual IActionResult GetGymMemberOtherDetails(int gymMemberDetailId)
        {
            try
            {
                GymMemberDetailsModel gymMemberDetailsModel = _generalGymMemberDetailsService.GetGymMemberOtherDetails(gymMemberDetailId);
                return IsNotNull(gymMemberDetailsModel) ? CreateOKResponse(new GymMemberDetailsResponse { GymMemberDetailsModel = gymMemberDetailsModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/UpdateGymMemberOtherDetails")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymMemberDetailsResponse))]
        public virtual IActionResult UpdateGymMemberOtherDetails([FromBody] GymMemberDetailsModel model)
        {
            try
            {
                bool isUpdated = _generalGymMemberDetailsService.UpdateGymMemberOtherDetails(model);
                return isUpdated ? CreateOKResponse(new GymMemberDetailsResponse { GymMemberDetailsModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/DeleteGymMembers")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGymMembers([FromBody] ParameterModel gymMemberDetailIds)
        {
            try
            {
                bool deleted = _generalGymMemberDetailsService.DeleteGymMembers(gymMemberDetailIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        #region Member Follow Up
        [HttpGet]
        [Route("/GymMemberDetails/GymMemberFollowUpList")]
        [Produces(typeof(GymMemberFollowUpListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GymMemberFollowUpList(int gymMemberDetailId, long personId, ExpandCollection expand, FilterCollection filter, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberFollowUpListModel list = _generalGymMemberDetailsService.GymMemberFollowUpList(gymMemberDetailId, personId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberFollowUpListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/GetGymMemberFollowUp")]
        [HttpGet]
        [Produces(typeof(GymMemberFollowUpResponse))]
        public virtual IActionResult GetGymMemberFollowUp(long gymMemberFollowUpId)
        {
            try
            {
                GymMemberFollowUpModel gymMemberFollowUpModel = _generalGymMemberDetailsService.GetGymMemberFollowUp(gymMemberFollowUpId);
                return IsNotNull(gymMemberFollowUpModel) ? CreateOKResponse(new GymMemberFollowUpResponse { GymMemberFollowUpModel = gymMemberFollowUpModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/InserUpdateGymMemberFollowUp")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymMemberFollowUpResponse))]
        public virtual IActionResult InserUpdateGymMemberFollowUp([FromBody] GymMemberFollowUpModel model)
        {
            try
            {
                bool isUpdated = _generalGymMemberDetailsService.InserUpdateGymMemberFollowUp(model);
                return isUpdated ? CreateOKResponse(new GymMemberFollowUpResponse { GymMemberFollowUpModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberFollowUpResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/DeleteGymMemberFollowUp")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGymMemberFollowUp([FromBody] ParameterModel countryIds)
        {
            try
            {
                bool deleted = _generalGymMemberDetailsService.DeleteGymMemberFollowUp(countryIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
        #endregion

        #region GymMemberMembershipPlan
        [HttpGet]
        [Route("/GymMemberDetails/GetGymMemberMembershipPlanList")]
        [Produces(typeof(GymMemberMembershipPlanListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGymMemberMembershipPlanList(int gymMemberDetailId, long personId, ExpandCollection expand, FilterCollection filter, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberMembershipPlanListModel list = _generalGymMemberDetailsService.GetGymMemberMembershipPlanList(gymMemberDetailId, personId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberMembershipPlanListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberMembershipPlanListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberMembershipPlanListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberDetails/AssociateGymMemberMembershipPlan")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymMemberMembershipPlanResponse))]
        public virtual IActionResult AssociateGymMemberMembershipPlan([FromBody] GymMemberMembershipPlanModel model)
        {
            try
            {
                GymMemberMembershipPlanModel gymMemberMembershipPlan = _generalGymMemberDetailsService.AssociateGymMemberMembershipPlan(model);
                return IsNotNull(gymMemberMembershipPlan) ? CreateCreatedResponse(new GymMemberMembershipPlanResponse { GymMemberMembershipPlanModel = gymMemberMembershipPlan }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/GymMemberDetails/GymMemberPaymentHistoryList")]
        [Produces(typeof(GymMemberSalesInvoiceListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GymMemberPaymentHistoryList(int gymMemberDetailId, long personId, ExpandCollection expand, FilterCollection filter, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberSalesInvoiceListModel list = _generalGymMemberDetailsService.GymMemberPaymentHistoryList(gymMemberDetailId, personId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
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
        #endregion
    }
}