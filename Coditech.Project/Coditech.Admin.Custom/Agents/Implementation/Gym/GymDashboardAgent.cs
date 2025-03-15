using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;

namespace Coditech.Admin.Agents
{
    public class GymDashboardAgent : BaseAgent, IGymDashboardAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymDashboardClient _dashboardClient;
        #endregion

        #region Public Constructor
        public GymDashboardAgent(ICoditechLogging coditechLogging, IGymDashboardClient dashboardClient)
        {
            _coditechLogging = coditechLogging;
            _dashboardClient = GetClient<IGymDashboardClient>(dashboardClient);
        }
        #endregion

        #region Public Methods

        //Get Gym Dashboard by general selected Admin Role Master id.
        public virtual GymDashboardViewModel GetGymDashboardDetails(short numberOfDaysRecord)
        {
            int selectedAdminRoleMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.SelectedAdminRoleMasterId ?? 0;
            long userMasterId = SessionHelper.GetDataFromSession<UserModel>(AdminConstants.UserDataSession)?.UserMasterId ?? 0;
            GymDashboardViewModel dashboardViewModel = new GymDashboardViewModel();
            if (selectedAdminRoleMasterId > 0 && userMasterId > 0)
            {
                GymDashboardResponse response = _dashboardClient.GetGymDashboardDetails(numberOfDaysRecord, selectedAdminRoleMasterId, userMasterId);
                dashboardViewModel = response?.GymDashboardModel?.ToViewModel<GymDashboardViewModel>();
            }
            dashboardViewModel.NumberOfDaysRecord = numberOfDaysRecord;
            return dashboardViewModel;
        }

        #endregion
    }
}
