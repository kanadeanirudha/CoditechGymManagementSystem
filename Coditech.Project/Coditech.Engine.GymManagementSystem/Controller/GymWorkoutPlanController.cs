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
    public class GymWorkoutPlanController : BaseController
    {
        private readonly IGymWorkoutPlanService _gymWorkoutPlanService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymWorkoutPlanController(ICoditechLogging coditechLogging, IGymWorkoutPlanService gymWorkoutPlanService)
        {
            _gymWorkoutPlanService = gymWorkoutPlanService;
            _coditechLogging = coditechLogging;
        }
        [HttpGet]
        [Route("/GymWorkoutPlan/GetGymWorkoutPlanList")]
        [Produces(typeof(GymWorkoutPlanListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGymWorkoutPlanList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymWorkoutPlanListModel list = _gymWorkoutPlanService.GetGymWorkoutPlanList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymWorkoutPlanListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        //Create
        [Route("/GymWorkoutPlan/CreateGymWorkoutPlan")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymWorkoutPlanResponse))]
        public virtual IActionResult CreateGymWorkoutPlan([FromBody] GymWorkoutPlanModel model)
        {
            try
            {
                GymWorkoutPlanModel gymWorkoutPlan = _gymWorkoutPlanService.CreateGymWorkoutPlan(model);
                return IsNotNull(gymWorkoutPlan) ? CreateCreatedResponse(new GymWorkoutPlanResponse { GymWorkoutPlanModel = gymWorkoutPlan }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }


        [Route("/GymWorkoutPlan/GetGymWorkoutPlan")]
        [HttpGet]
        [Produces(typeof(GymWorkoutPlanResponse))]
        public virtual IActionResult GetGymWorkoutPlan(long gymWorkoutPlanId)
        {
            try
            {
                GymWorkoutPlanModel gymWorkoutPlanModel = _gymWorkoutPlanService.GetGymWorkoutPlan(gymWorkoutPlanId);
                return IsNotNull(gymWorkoutPlanModel) ? CreateOKResponse(new GymWorkoutPlanResponse { GymWorkoutPlanModel = gymWorkoutPlanModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymWorkoutPlan/UpdateGymWorkoutPlan")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymWorkoutPlanResponse))]
        public virtual IActionResult UpdateGymWorkoutPlan([FromBody] GymWorkoutPlanModel model)
        {
            try
            {
                bool isUpdated = _gymWorkoutPlanService.UpdateGymWorkoutPlan(model);
                return isUpdated ? CreateOKResponse(new GymWorkoutPlanResponse { GymWorkoutPlanModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymWorkoutPlan/GetWorkoutPlanDetails")]
        [HttpGet]
        [Produces(typeof(GymWorkoutPlanResponse))]
        public virtual IActionResult GetWorkoutPlanDetails(long gymWorkoutPlanId)
        {
            try
            {
                GymWorkoutPlanModel gymWorkoutPlanModel = _gymWorkoutPlanService.GetWorkoutPlanDetails(gymWorkoutPlanId);
                return IsNotNull(gymWorkoutPlanModel) ? CreateOKResponse(new GymWorkoutPlanResponse { GymWorkoutPlanModel = gymWorkoutPlanModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymWorkoutPlan/AddWorkoutPlanDetails")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymWorkoutPlanDetailsResponse))]
        public virtual IActionResult AddWorkoutPlanDetails([FromBody] GymWorkoutPlanDetailsModel model)
        {
            try
            {
                GymWorkoutPlanDetailsModel gymWorkoutPlanSet = _gymWorkoutPlanService.AddWorkoutPlanDetails(model);
                return IsNotNull(gymWorkoutPlanSet) ? CreateCreatedResponse(new GymWorkoutPlanDetailsResponse { GymWorkoutPlanDetailsModel = gymWorkoutPlanSet }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanDetailsResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanDetailsResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymWorkoutPlan/DeleteGymWorkoutPlanDetails")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGymWorkoutPlanDetails([FromBody] DeleteWorkoutPlanDetailsModel gymWorkoutPlanIds)
        {
            try
            {
                bool deleted = _gymWorkoutPlanService.DeleteGymWorkoutPlanDetails(gymWorkoutPlanIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/GymWorkoutPlan/GetAssociatedMemberList")]
        [Produces(typeof(GymWorkoutPlanUserListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetAssociatedMemberList(long gymWorkoutPlanId, FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymWorkoutPlanUserListModel list = _gymWorkoutPlanService.GetAssociatedMemberList(gymWorkoutPlanId, filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymWorkoutPlanUserListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanUserListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanUserListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
       
        [Route("/GymWorkoutPlan/AssociateUnAssociateWorkoutPlanUser")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymWorkoutPlanUserResponse))]
        public virtual IActionResult AssociateUnAssociateWorkoutPlanUser([FromBody] GymWorkoutPlanUserModel model)
        {
            try
            {
                bool isUpdated = _gymWorkoutPlanService.AssociateUnAssociateWorkoutPlanUser(model);
                return isUpdated ? CreateOKResponse(new GymWorkoutPlanUserResponse { GymWorkoutPlanUserModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanUserResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymWorkoutPlanUserResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}