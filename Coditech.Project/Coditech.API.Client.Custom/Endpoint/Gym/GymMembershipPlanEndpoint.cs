using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymMembershipPlanEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMembershipPlan/GetGymMembershipPlanList?selectedCentreCode={selectedCentreCode}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string CreateGymMembershipPlanAsync() =>
           $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMembershipPlan/CreateGymMembershipPlan";

        public string GetGymMembershipPlanAsync(int gymMembershipPlanId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMembershipPlan/GetGymMembershipPlan?gymMembershipPlanId={gymMembershipPlanId}";

        public string UpdateGymMembershipPlanAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMembershipPlan/UpdateGymMembershipPlan";

        public string DeleteGymMembershipPlanAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMembershipPlan/DeleteGymMembershipPlan";
    }
}
