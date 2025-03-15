using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface IGymDashboardClient : IBaseClient
    {
        /// <summary>
        /// Get Gym Dashboard by selectedAdminRoleMasterId.
        /// </summary>
        /// <param name="selectedAdminRoleMasterId">selectedAdminRoleMasterId</param>
        /// <param name="userMasterId">userMasterId</param>
        /// <returns>Returns GymDashboardResponse.</returns>
        GymDashboardResponse GetGymDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId, long userMasterId);
    }
}
