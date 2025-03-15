using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymMemberBodyMeasurementEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/GetMemberBodyMeasurementList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string GetBodyMeasurementTypeListByMemberIdAsync(int gymMemberDetailId, long personId, short pageSize) =>
           $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/GetBodyMeasurementTypeListByMemberId?gymMemberDetailId={gymMemberDetailId}&personId={personId}&pageSize={pageSize}";

        public string CreateMemberBodyMeasurementAsync() =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/CreateMemberBodyMeasurement";

        public string GetMemberBodyMeasurementAsync(long GymMemberBodyMeasurementId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/GetMemberBodyMeasurement?GymMemberBodyMeasurementId={GymMemberBodyMeasurementId}";
       
        public string UpdateMemberBodyMeasurementAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/UpdateMemberBodyMeasurement";

        public string DeleteMemberBodyMeasurementAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymMemberBodyMeasurement/DeleteMemberBodyMeasurement";
    }
}
