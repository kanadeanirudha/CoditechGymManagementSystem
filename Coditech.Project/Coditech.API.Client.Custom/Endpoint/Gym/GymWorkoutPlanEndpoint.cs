using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymWorkoutPlanEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/GetGymWorkoutPlanList?selectedCentreCode={selectedCentreCode}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string CreateGymWorkoutPlanAsync() =>
           $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/CreateGymWorkoutPlan";
        public string GetGymWorkoutPlanAsync(long gymWorkoutPlanId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/GetGymWorkoutPlan?gymWorkoutPlanId={gymWorkoutPlanId}";

        public string UpdateGymWorkoutPlanAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/UpdateGymWorkoutPlan";

        public string DeleteGymWorkoutPlanAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/DeleteGymWorkoutPlan";

        public string GetWorkoutPlanDetailsAsync(long gymWorkoutPlanId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/GetWorkoutPlanDetails?gymWorkoutPlanId={gymWorkoutPlanId}";

        public string AddWorkoutPlanDetailsAsync() =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/AddWorkoutPlanDetails";

        public string DeleteGymWorkoutPlanDetailsAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/DeleteGymWorkoutPlanDetails";

        public string GetAssociatedMemberListAsync(long gymWorkoutPlanId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/GetAssociatedMemberList?gymWorkoutPlanId={gymWorkoutPlanId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string AssociateUnAssociateWorkoutPlanUserAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymWorkoutPlan/AssociateUnAssociateWorkoutPlanUser";
        
    }
}
