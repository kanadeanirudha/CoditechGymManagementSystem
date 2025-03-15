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
    public class GymBodyMeasurementTypeController : BaseController
    {
        private readonly IGymBodyMeasurementTypeService _gymBodyMeasurementTypeService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymBodyMeasurementTypeController(ICoditechLogging coditechLogging, IGymBodyMeasurementTypeService gymBodyMeasurementTypeService)
        {
            _gymBodyMeasurementTypeService = gymBodyMeasurementTypeService;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/GymBodyMeasurementType/GetGymBodyMeasurementTypeList")]
        [Produces(typeof(GymBodyMeasurementTypeListResponse))]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult GetGymBodyMeasurementTypeList(FilterCollection filter, ExpandCollection expand, SortCollection sort, int pageIndex, int pageSize)
        {
            try
            {
                GymBodyMeasurementTypeListModel list = _gymBodyMeasurementTypeService.GetGymBodyMeasurementTypeList(filter, sort.ToNameValueCollectionSort(), expand.ToNameValueCollectionExpands(), pageIndex, pageSize);
                string data = ApiHelper.ToJson(list);
                return !string.IsNullOrEmpty(data) ? CreateOKResponse<GymBodyMeasurementTypeListResponse>(data) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeListResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeListResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymBodyMeasurementType/CreateGymBodyMeasurementType")]
        [HttpPost, ValidateModel]
        [Produces(typeof(GymBodyMeasurementTypeResponse))]
        public virtual IActionResult CreateGymBodyMeasurementType([FromBody] GymBodyMeasurementTypeModel model)
        {
            try
            {
                GymBodyMeasurementTypeModel gymBodyMeasurementType = _gymBodyMeasurementTypeService.CreateGymBodyMeasurementType(model);
                return IsNotNull(gymBodyMeasurementType) ? CreateCreatedResponse(new GymBodyMeasurementTypeResponse { GymBodyMeasurementTypeModel = gymBodyMeasurementType }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymBodyMeasurementType/GetGymBodyMeasurementType")]
        [HttpGet]
        [Produces(typeof(GymBodyMeasurementTypeResponse))]
        public virtual IActionResult GetGymBodyMeasurementType(short gymBodyMeasurementTypeId)
        {
            try
            {
                GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel = _gymBodyMeasurementTypeService.GetGymBodyMeasurementType(gymBodyMeasurementTypeId);
                return IsNotNull(gymBodyMeasurementTypeModel) ? CreateOKResponse(new GymBodyMeasurementTypeResponse { GymBodyMeasurementTypeModel = gymBodyMeasurementTypeModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymBodyMeasurementType/UpdateGymBodyMeasurementType")]
        [HttpPut, ValidateModel]
        [Produces(typeof(GymBodyMeasurementTypeResponse))]
        public virtual IActionResult UpdateGymBodyMeasurementType([FromBody] GymBodyMeasurementTypeModel model)
        {
            try
            {
                bool isUpdated = _gymBodyMeasurementTypeService.UpdateGymBodyMeasurementType(model);
                return isUpdated ? CreateOKResponse(new GymBodyMeasurementTypeResponse { GymBodyMeasurementTypeModel = model }) : CreateInternalServerErrorResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymBodyMeasurementTypeResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }

        [Route("/GymBodyMeasurementType/DeleteGymBodyMeasurementType")]
        [HttpPost, ValidateModel]
        [Produces(typeof(TrueFalseResponse))]
        public virtual IActionResult DeleteGymBodyMeasurementType([FromBody] ParameterModel measurementTypeIds)
        {
            try
            {
                bool deleted = _gymBodyMeasurementTypeService.DeleteGymBodyMeasurementType(measurementTypeIds);
                return CreateOKResponse(new TrueFalseResponse { IsSuccess = deleted });
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return CreateInternalServerErrorResponse(new TrueFalseResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}