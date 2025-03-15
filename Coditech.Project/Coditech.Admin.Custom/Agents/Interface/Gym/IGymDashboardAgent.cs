using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IGymDashboardAgent
    {
        /// <summary>
        /// Get GetGymDashboardDetails.
        /// </summary>
        /// <returns>Returns GymDashboardViewModel.</returns>
        GymDashboardViewModel GetGymDashboardDetails(short numberOfDaysRecord);
    }
}
