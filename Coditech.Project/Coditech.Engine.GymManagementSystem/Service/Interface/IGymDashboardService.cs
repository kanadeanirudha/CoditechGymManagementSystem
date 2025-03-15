using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IGymDashboardService
    {
        GymDashboardModel GetGymDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId, long userMasterId);
    }
}
