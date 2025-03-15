using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class GymDashboardEndpoint : BaseEndpoint
    {
        public string GetGymDashboardDetailsAsync(short numberOfDaysRecord, int selectedAdminRoleMasterId,long userMasterId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymDashboardController/GetGymDashboardDetails?numberOfDaysRecord={numberOfDaysRecord}&selectedAdminRoleMasterId={selectedAdminRoleMasterId}&userMasterId={userMasterId}";
    }
}
