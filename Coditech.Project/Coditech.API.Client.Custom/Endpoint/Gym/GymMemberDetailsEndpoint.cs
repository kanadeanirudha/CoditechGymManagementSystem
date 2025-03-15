using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymMemberDetailsEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GetGymMemberDetailsList?selectedCentreCode={selectedCentreCode}{BuildEndpointQueryString(true,expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetGymMemberOtherDetailsAsync(int gymMemberDetailId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GetGymMemberOtherDetails?gymMemberDetailId={gymMemberDetailId}";

        public string UpdateGymMemberOtherDetailsAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/UpdateGymMemberOtherDetails";

        public string DeleteGymMembersAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/DeleteGymMembers";


        #region Gym Member FollowUp
        public string GymMemberFollowUpListAsync(int gymMemberDetailId, long personId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GymMemberFollowUpList?gymMemberDetailId={gymMemberDetailId}&personId={personId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string GetGymMemberFollowUpAsync(long gymMemberFollowUpId) =>
          $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GetGymMemberFollowUp?gymMemberFollowUpId={gymMemberFollowUpId}";

        public string InserUpdateGymMemberFollowUpAsync() =>
              $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/InserUpdateGymMemberFollowUp";

        public string DeleteGymMemberFollowUpAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/DeleteGymMemberFollowUp";
        #endregion

        #region Gym Member Membership Plan
        public string GetGymMemberMembershipPlanListAsync(int gymMemberDetailId, long personId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GetGymMemberMembershipPlanList?gymMemberDetailId={gymMemberDetailId}&personId={personId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string AssociateGymMemberMembershipPlanAsync() =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/AssociateGymMemberMembershipPlan";

        public string GymMemberPaymentHistoryListAsync(int gymMemberDetailId, long personId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberDetails/GymMemberPaymentHistoryList?gymMemberDetailId={gymMemberDetailId}&personId={personId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        #endregion
    }
}
