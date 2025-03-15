using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IGymBodyMeasurementTypeAgent
    {
        /// <summary>
        /// Get list of GymBodyMeasurementType.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymBodyMeasurementTypeListViewModel</returns>
        GymBodyMeasurementTypeListViewModel GetGymBodyMeasurementTypeList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create GymBodyMeasurementType.
        /// </summary>
        /// <param name="gymBodyMeasurementTypeViewModel">Gym Body Measurement Type View Model.</param>
        /// <returns>Returns created model.</returns>
        GymBodyMeasurementTypeViewModel CreateGymBodyMeasurementType(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel);

        /// <summary>
        /// Get GymBodyMeasurementType by gymBodyMeasurementTypeId.
        /// </summary>
        /// <param name="gymBodyMeasurementTypeId">gymBodyMeasurementTypeId</param>
        /// <returns>Returns GymBodyMeasurementTypeViewModel.</returns>
        GymBodyMeasurementTypeViewModel GetGymBodyMeasurementType(short gymBodyMeasurementTypeId);

        /// <summary>
        /// Update GymBodyMeasurementType.
        /// </summary>
        /// <param name="gymBodyMeasurementTypeViewModel">gymBodyMeasurementTypeViewModel.</param>
        /// <returns>Returns updated GymBodyMeasurementTypeViewModel</returns>
        GymBodyMeasurementTypeViewModel UpdateGymBodyMeasurementType(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel);

        /// <summary>
        /// Delete GymBodyMeasurementType.
        /// </summary>
        /// <param name="gymBodyMeasurementTypeId">gymBodyMeasurementTypeId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteGymBodyMeasurementType(string gymBodyMeasurementTypeId, out string errorMessage);
        GymBodyMeasurementTypeListResponse GetGymBodyMeasurementTypeList();
    }
}
