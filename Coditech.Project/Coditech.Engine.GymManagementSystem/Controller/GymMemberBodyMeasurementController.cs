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
    public class GymMemberBodyMeasurementController : BaseController
    {
        private readonly IGymMemberBodyMeasurementService _gymMemberBodyMeasurementService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymMemberBodyMeasurementController(ICoditechLogging coditechLogging, IGymMemberBodyMeasurementService gymMemberBodyMeasurementService)
        {
            _gymMemberBodyMeasurementService = gymMemberBodyMeasurementService;
            _coditechLogging = coditechLogging;
        }


        [HttpGet]
        [Route("/GymMemberBodyMeasurement/GetBodyMeasurementTypeListByMemberId")]
        [Produces(typeof(GymMemberBodyMeasurementListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId, short pageSize)
        {
            try
            {
                GymMemberBodyMeasurementListModel list = _gymMemberBodyMeasurementService.GetBodyMeasurementTypeListByMemberId(gymMemberDetailId, personId, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberBodyMeasurementListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("/GymMemberBodyMeasurement/GetMemberBodyMeasurementList")]
        [Produces(typeof(GymMemberBodyMeasurementListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetMemberBodyMeasurementList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymMemberBodyMeasurementListModel list = _gymMemberBodyMeasurementService.GetMemberBodyMeasurementList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymMemberBodyMeasurementListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }


        [Route("/GymMemberBodyMeasurement/CreateMemberBodyMeasurement")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymMemberBodyMeasurementResponse))]
        public virtual IActionResult CreateMemberBodyMeasurement([FromBody] GymMemberBodyMeasurementModel model)
        {
            try
            {
                GymMemberBodyMeasurementModel MemberBodyMeasurement = _gymMemberBodyMeasurementService.CreateMemberBodyMeasurement(model);
                return IsNotNull(MemberBodyMeasurement) ? CreateCreatedResponse(new GymMemberBodyMeasurementResponse { GymMemberBodyMeasurementModel = MemberBodyMeasurement }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberBodyMeasurement/GetMemberBodyMeasurement")]
        [HttpGet]
        [Produces(typeof(GymMemberBodyMeasurementResponse))]
        public virtual IActionResult GetMemberBodyMeasurement(long gymMemberBodyMeasurementId)
        {
            try
            {
                GymMemberBodyMeasurementModel gymMemberBodyMeasurementModel = _gymMemberBodyMeasurementService.GetMemberBodyMeasurement(gymMemberBodyMeasurementId);
                return IsNotNull(gymMemberBodyMeasurementModel) ? CreateOKResponse(new GymMemberBodyMeasurementResponse { GymMemberBodyMeasurementModel = gymMemberBodyMeasurementModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberBodyMeasurement/UpdateMemberBodyMeasurement")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymMemberBodyMeasurementResponse))]
        public virtual IActionResult UpdateMemberBodyMeasurement([FromBody] GymMemberBodyMeasurementModel model)
        {
            try
            {
                bool isUpdated = _gymMemberBodyMeasurementService.UpdateMemberBodyMeasurement(model);
                return isUpdated ? CreateOKResponse(new GymMemberBodyMeasurementResponse { GymMemberBodyMeasurementModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymMemberBodyMeasurementResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymMemberBodyMeasurement/DeleteMemberBodyMeasurement")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteMemberBodyMeasurement([FromBody] ParameterModel MemberBodyMeasurementIds)
        {
            try
            {
                bool deleted = _gymMemberBodyMeasurementService.DeleteMemberBodyMeasurement(MemberBodyMeasurementIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.MemberBodyMeasurement.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}