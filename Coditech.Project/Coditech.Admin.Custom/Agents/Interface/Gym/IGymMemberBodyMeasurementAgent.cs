using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IGymMemberBodyMeasurementAgent
    {
        /// <summary>
        /// Get Body Measurement Type List By MemberId
        /// </summary>
        /// <param name="gymMemberDetailId">gymMemberDetailId</param>
        /// <param name="personId">personId</param>
        /// <param name="listCount">listCount</param>
        /// <returns>GymMemberBodyMeasurementListViewModel</returns>
        GymMemberBodyMeasurementListViewModel GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId, short pageSize);

        /// <summary>
        /// Get list of General MemberBodyMeasurement.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberBodyMeasurementListViewModel</returns>
        GymMemberBodyMeasurementListViewModel GetMemberBodyMeasurementList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create MemberBodyMeasurement.
        /// </summary>
        /// <param name="GymMemberBodyMeasurementViewModel">General MemberBodyMeasurement View Model.</param>
        /// <returns>Returns created model.</returns>
        GymMemberBodyMeasurementViewModel CreateMemberBodyMeasurement(GymMemberBodyMeasurementViewModel GymMemberBodyMeasurementViewModel);

        /// <summary>
        /// Get MemberBodyMeasurement by GymMemberBodyMeasurementId and GymBodyMeasurementTypeId.
        /// </summary>
        /// <param name="gymMemberBodyMeasurementId">The ID of the gym member body measurement.</param>
        /// <param name="gymBodyMeasurementTypeId">The ID of the gym body measurement type.</param>
        /// <returns>Returns GymMemberBodyMeasurementViewModel.</returns>
        GymMemberBodyMeasurementViewModel GetMemberBodyMeasurement(long gymMemberBodyMeasurementId, short gymBodyMeasurementTypeId);


        /// <summary>
        /// Update MemberBodyMeasurement.
        /// </summary>
        /// <param name="GymMemberBodyMeasurementViewModel">GymMemberBodyMeasurementViewModel.</param>
        /// <returns>Returns updated GymMemberBodyMeasurementViewModel</returns>
        GymMemberBodyMeasurementViewModel UpdateMemberBodyMeasurement(GymMemberBodyMeasurementViewModel GymMemberBodyMeasurementViewModel);

        /// <summary>
        /// Delete MemberBodyMeasurement.
        /// </summary>
        /// <param name="GymMemberBodyMeasurementId">GymMemberBodyMeasurementId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteMemberBodyMeasurement(string GymMemberBodyMeasurementId, out string errorMessage);
        GymMemberBodyMeasurementListResponse GetMemberBodyMeasurementList();
    }
}
