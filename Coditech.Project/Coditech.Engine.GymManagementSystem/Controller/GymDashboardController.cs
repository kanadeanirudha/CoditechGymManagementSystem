using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Logger;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Controllers
{

    public class GymDashboardController : BaseController
    {

        private readonly IGymDashboardService _dashboardService;
        protected readonly ICoditechLogging _coditechLogging;
        public GymDashboardController(ICoditechLogging coditechLogging, IGymDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            _coditechLogging = coditechLogging;
        }

        [Route("/GymDashboardController/GetGymDashboardDetails")]
        [HttpGet]
        [Produces(typeof(GymDashboardResponse))]
        public virtual IActionResult GetGymDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId,long userMasterId)
        {
            try
            {
                GymDashboardModel dashboardModel = _dashboardService.GetGymDashboardDetails(numberOfDaysRecord, selectedAdminRoleMasterId, userMasterId);
                return IsNotNull(dashboardModel) ? CreateOKResponse(new GymDashboardResponse { GymDashboardModel = dashboardModel }) : CreateNoContentResponse();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Warning);
                return CreateInternalServerErrorResponse(new GymDashboardResponse { HasError = true, ErrorMessage = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, CoditechLoggingEnum.Components.Dashboard.ToString(), TraceLevel.Error);
                return CreateInternalServerErrorResponse(new GymDashboardResponse { HasError = true, ErrorMessage = ex.Message });
            }
        }
    }
}