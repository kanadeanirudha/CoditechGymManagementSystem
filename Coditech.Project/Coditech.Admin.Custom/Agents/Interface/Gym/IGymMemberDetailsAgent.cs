using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IGymMemberDetailsAgent
    {
        /// <summary>
        /// Get list of Gym Member.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberDetailsListViewModel</returns>
        GymMemberDetailsListViewModel GetGymMemberDetailsList(DataTableViewModel dataTableModel, string listType = null);

        /// <summary>
        /// Create MemberDetails.
        /// </summary>
        /// <param name="gymCreateEditMemberViewModel">Gym CreateEdi tMember View Model.</param>
        /// <returns>Returns created model.</returns>
        GymCreateEditMemberViewModel CreateMemberDetails(GymCreateEditMemberViewModel gymCreateEditMemberViewModel);

        /// <summary>
        /// Get Member Personal Details by personId.
        /// </summary>
        /// <param name="personId">personId</param>
        /// <returns>Returns GymCreateEditMemberViewModel.</returns>
        GymCreateEditMemberViewModel GetMemberPersonalDetails(int gymMemberDetailId, long personId);

        /// <summary>
        /// Update Member Personal Details.
        /// </summary>
        /// <param name="gymCreateEditMemberViewModel">gymCreateEditMemberViewModel.</param>
        /// <returns>Returns updated GymCreateEditMemberViewModel</returns>
        GymCreateEditMemberViewModel UpdateMemberPersonalDetails(GymCreateEditMemberViewModel gymCreateEditMemberViewModel);

        /// <summary>
        /// Get MemberOtherDetails by gymMemberDetailId.
        /// </summary>
        /// <param name="gymMemberDetailId">gymMemberDetailId</param>
        /// <returns>Returns GymMemberDetailsResponse.</returns>
        GymMemberDetailsViewModel GetGymMemberOtherDetails(int gymMemberDetailId);

        /// <summary>
        /// Update GymMember Other Details
        /// </summary>
        /// <param name="GymMemberDetailsModel">GymMemberDetailsModel.</param>
        /// <returns>Returns updated GymMemberDetailsViewModel</returns>
        GymMemberDetailsViewModel UpdateGymMemberOtherDetails(GymMemberDetailsViewModel gymMemberDetailsModel);
        /// <summary>
        /// Delete Gym Members.
        /// </summary>
        /// <param name="gymMemberDetailIds">gymMemberDetailIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteGymMembers(string gymMemberDetailIds, out string errorMessage);

        /// <summary>
        /// Get list of Gym Member.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberFollowUpListViewModel</returns>
        GymMemberFollowUpListViewModel GymMemberFollowUpList(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel);

        /// <summary>
        /// Get GetMemberFollowUp by gymMemberFollowUpId.
        /// </summary>
        /// <param name="gymMemberDetailId">gymMemberFollowUpId</param>
        /// <returns>Returns GymMemberFollowUpViewModel.</returns>
        GymMemberFollowUpViewModel GetMemberFollowUp(long gymMemberFollowUpId);

        /// <summary>
        /// Inser Update Member FollowUp
        /// </summary>
        /// <param name="GymMemberFollowUpViewModel">GymMemberFollowUpViewModel.</param>
        /// <returns>Returns updated GymMemberFollowUpViewModel</returns>
        GymMemberFollowUpViewModel InserUpdateGymMemberFollowUp(GymMemberFollowUpViewModel gymMemberFollowUpViewModel);

        /// <summary>
        /// Delete Gym Members Follow Up.
        /// </summary>
        /// <param name="gymMemberFollowUpIdIds">gymMemberFollowUpIdIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteGymMemberFollowUp(string gymMemberFollowUpIdIds, out string errorMessage);

        /// <summary>
        /// Get list of Gym Member.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GeneralPersonAttendanceDetailsListViewModel</returns>
        GeneralPersonAttendanceDetailsListViewModel GeneralPersonAttendanceDetailsList(int gymMemberDetailId, long personId, string userType, DataTableViewModel dataTableModel);

        /// <summary>
        /// Get GetGeneralPersonAttendanceDetails by generalPersonAttendanceDetailsId.
        /// </summary>
        /// <param name="gymMemberDetailId">generalPersonAttendanceDetailsId</param>
        /// <returns>Returns GeneralPersonAttendanceDetailsViewModel.</returns>
        GeneralPersonAttendanceDetailsViewModel GetGeneralPersonAttendanceDetails(long generalPersonAttendanceDetailsId);

        /// <summary>
        /// Inser Update General Person Attendance Details
        /// </summary>
        /// <param name="GeneralPersonAttendanceDetailsViewModel">GeneralPersonAttendanceDetailsViewModel.</param>
        /// <returns>Returns updated GeneralPersonAttendanceDetailsViewModel</returns>
        GeneralPersonAttendanceDetailsViewModel InserUpdateGeneralPersonAttendanceDetails(GeneralPersonAttendanceDetailsViewModel generalPersonAttendanceDetailsViewModel);

        /// <summary>
        /// Delete General Person Attendance Details.
        /// </summary>
        /// <param name="generalPersonAttendanceDetailsIdIds">generalPersonAttendanceDetailsIdIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteGeneralPersonAttendanceDetails(string generalPersonAttendanceDetailsIdIds, out string errorMessage);

        /// <summary>
        /// Get list of Gym Member Membership Plan.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberMembershipPlanListViewModel</returns>
        GymMemberMembershipPlanListViewModel GetGymMemberMembershipPlan(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel);

        /// <summary>
        /// Associate Gym Member Membership Plan
        /// </summary>
        /// <param name="GymMemberMembershipPlanViewModel">GymMemberMembershipPlanViewModel.</param>
        /// <returns>Returns updated GymMemberMembershipPlanViewModel</returns>
        GymMemberMembershipPlanViewModel AssociateGymMemberMembershipPlan(GymMemberMembershipPlanViewModel gymMemberMembershipPlanViewModel);

        /// <summary>
        /// Get list of Gym Member Payment History.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymMemberSalesInvoiceListViewModel</returns>
        GymMemberSalesInvoiceListViewModel GymMemberPaymentHistoryList(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel);
    }
}
