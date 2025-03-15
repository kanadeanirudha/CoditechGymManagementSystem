using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class GymBodyMeasurementTypeEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymBodyMeasurementType/GetGymBodyMeasurementTypeList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string CreateGymBodyMeasurementTypeAsync() =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymBodyMeasurementType/CreateGymBodyMeasurementType";

        public string GetGymBodyMeasurementTypeAsync(short gymBodyMeasurementTypeId) =>
            $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymBodyMeasurementType/GetGymBodyMeasurementType?gymBodyMeasurementTypeId={gymBodyMeasurementTypeId}";
       
        public string UpdateGymBodyMeasurementTypeAsync() =>
               $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymBodyMeasurementType/UpdateGymBodyMeasurementType";

        public string DeleteGymBodyMeasurementTypeAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechGymManagementSystemApiRootUri}/GymBodyMeasurementType/DeleteGymBodyMeasurementType";
    }
}
