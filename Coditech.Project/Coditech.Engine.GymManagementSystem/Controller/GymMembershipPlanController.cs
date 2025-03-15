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
    public class GymMembershipPlanController : BaseController
    {
        private readonly IGymMembershipPlanService _generalGymMembershipPlanService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymMembershipPlanController(ICoditechLogging coditechLogging, IGymMembershipPlanService generalGymMembershipPlanService)
        {
            _generalGymMembershipPlanService = generalGymMembershipPlanService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/GymMembershipPlan/GetGymMembershipPlanList")]
        [Produces(typeof(GymMembershipPlanListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGymMembershipPlanList(string selectedCentreCode, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMembershipPlanListModel list = _generalGymMembershipPlanService.GetGymMembershipPlanList(selectedCentreCode, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMembershipPlanListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMembershipPlanListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMembershipPlanListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }


        [Route("/GymMembershipPlan/CreateGymMembershipPlan")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymMembershipPlanResponse))]
        public virtual IActionResult CreateGymMembershipPlan([FromBody] GymMembershipPlanModel model)
        {
            try
            {
                GymMembershipPlanModel gymMembershipPlanModel = _generalGymMembershipPlanService.CreateGymMembershipPlan(model);
                return IsNotNull(gymMembershipPlanModel) ? CreateCreatedResponse(new GymMembershipPlanResponse { GymMembershipPlanModel = gymMembershipPlanModel }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }


        [Route("/GymMembershipPlan/GetGymMembershipPlan")]
        [HttpGet]
        [Produces(typeof(GymMembershipPlanResponse))]
        public virtual IActionResult GetGymMembershipPlan(int gymMembershipPlanId)
        {
            try
            {
                GymMembershipPlanModel gymMembershipPlanModel = _generalGymMembershipPlanService.GetGymMembershipPlan(gymMembershipPlanId);
                return IsNotNull(gymMembershipPlanModel) ? CreateOKResponse(new GymMembershipPlanResponse { GymMembershipPlanModel = gymMembershipPlanModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMembershipPlan/UpdateGymMembershipPlan")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymMembershipPlanResponse))]
        public virtual IActionResult UpdateGymMembershipPlan([FromBody] GymMembershipPlanModel model)
        {
            try
            {
                bool isUpdated = _generalGymMembershipPlanService.UpdateGymMembershipPlan(model);
                return isUpdated ? CreateOKResponse(new GymMembershipPlanResponse { GymMembershipPlanModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMembershipPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMembershipPlan/DeleteGymMembershipPlan")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGymMembershipPlan([FromBody] ParameterModel gymMembershipPlanIds)
        {
            try
            {
                bool deleted = _generalGymMembershipPlanService.DeleteGymMembershipPlan(gymMembershipPlanIds);
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
    }
}