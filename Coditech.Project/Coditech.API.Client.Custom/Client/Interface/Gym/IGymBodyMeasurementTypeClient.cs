using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IGymBodyMeasurementTypeClient : IBaseClient
    {
        /// <summary>
        /// Get list of GymBodyMeasurementType.
        /// </summary>
        /// <returns>GymBodyMeasurementTypeListResponse</returns>
        GymBodyMeasurementTypeListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create GymBodyMeasurementType.
        /// </summary>
        /// <param name="GymBodyMeasurementTypeModel">GymBodyMeasurementTypeModel.</param>
        /// <returns>Returns GymBodyMeasurementTypeResponse.</returns>
        GymBodyMeasurementTypeResponse CreateGymBodyMeasurementType(GymBodyMeasurementTypeModel body);

        /// <summary>
        /// Get GymBodyMeasurementType by gymBodyMeasurementTypeId.
        /// </summary>
        /// <param name="gymBodyMeasurementTypeId">gymBodyMeasurementTypeId</param>
        /// <returns>Returns GymBodyMeasurementTypeResponse.</returns>
        GymBodyMeasurementTypeResponse GetGymBodyMeasurementType(short gymBodyMeasurementTypeId);

        /// <summary>
        /// Update GymBodyMeasurementType.
        /// </summary>
        /// <param name="GymBodyMeasurementTypeModel">GymBodyMeasurementTypeModel.</param>
        /// <returns>Returns updated GymBodyMeasurementTypeResponse</returns>
        GymBodyMeasurementTypeResponse UpdateGymBodyMeasurementType(GymBodyMeasurementTypeModel body);

        /// <summary>
        /// Delete GymBodyMeasurementType.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteGymBodyMeasurementType(ParameterModel body);
    }
}
