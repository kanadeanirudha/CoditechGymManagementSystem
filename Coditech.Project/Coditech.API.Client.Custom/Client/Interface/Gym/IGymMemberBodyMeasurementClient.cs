using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IGymMemberBodyMeasurementClient : IBaseClient
    {
        /// <summary>
        /// Get Body Measurement Type List By MemberId
        /// </summary>
        /// <param name="gymMemberDetailId">gymMemberDetailId</param>
        /// <param name="personId">personId</param>
        /// <param name="listCount">listCount</param>
        /// <returns>GymMemberBodyMeasurementListViewModel</returns>
        GymMemberBodyMeasurementListResponse GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId, short pageSize);

        /// <summary>
        /// Get list of  MemberBodyMeasurement.
        /// </summary>
        /// <returns>GymMemberBodyMeasurementListResponse</returns>
        GymMemberBodyMeasurementListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create MemberBodyMeasurement.
        /// </summary>
        /// <param name="GymMemberBodyMeasurementModel">GymMemberBodyMeasurementModel.</param>
        /// <returns>Returns GymMemberBodyMeasurementResponse.</returns>
        GymMemberBodyMeasurementResponse CreateMemberBodyMeasurement(GymMemberBodyMeasurementModel body);

        /// <summary>
        /// Retrieves a gym member body measurement by its ID and gym body measurement type ID.
        /// </summary>
        /// <param name="gymMemberBodyMeasurementId">The ID of the gym member body measurement.</param>
        /// <param name="gymBodyMeasurementTypeId">The ID of the gym body measurement type.</param>
        /// <returns>Returns a response containing the gym member body measurement.</returns>
        GymMemberBodyMeasurementResponse GetMemberBodyMeasurement(long GymMemberBodyMeasurementId, short gymBodyMeasurementTypeId);


        /// <summary>
        /// Update MemberBodyMeasurement.
        /// </summary>
        /// <param name="GymMemberBodyMeasurementModel">GymMemberBodyMeasurementModel.</param>
        /// <returns>Returns updated GymMemberBodyMeasurementResponse</returns>
        GymMemberBodyMeasurementResponse UpdateMemberBodyMeasurement(GymMemberBodyMeasurementModel body);

        /// <summary>
        /// Delete MemberBodyMeasurement.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteMemberBodyMeasurement(ParameterModel body);
    }
}
