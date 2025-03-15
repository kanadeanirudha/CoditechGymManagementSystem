using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IGymWorkoutPlanAgent
    {
        /// <summary>
        /// Get list of GymWorkoutPlan.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymWorkoutPlanListViewModel</returns>
        GymWorkoutPlanListViewModel GetGymWorkoutPlanList(DataTableViewModel dataTableModel, string listType = null);

        /// <summary>
        /// Create GymWorkoutPlan.
        /// </summary>
        /// <param name="gymWorkoutPlanViewModel">Gym Workout Plan View Model.</param>
        /// <returns>Returns created model.</returns>
        GymWorkoutPlanViewModel CreateGymWorkoutPlan(GymWorkoutPlanViewModel gymWorkoutPlanViewModel);

        /// <summary>
        /// Get GymWorkoutPlan by gymWorkoutPlanId.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanId</param>
        /// <returns>Returns GymWorkoutPlanResponse.</returns>
        GymWorkoutPlanViewModel GetGymWorkoutPlan(long gymWorkoutPlanId);

        /// <summary>
        /// Update Gym Workout Plan
        /// </summary>
        /// <param name="gymWorkoutPlanModel">GymWorkoutPlanModel.</param>
        /// <returns>Returns updated GymWorkoutPlanViewModel</returns>
        GymWorkoutPlanViewModel UpdateGymWorkoutPlan(GymWorkoutPlanViewModel gymWorkoutPlanModel);

        #region GymWorkoutPlanDetails

        /// <summary>
        /// Get WorkoutPlan by GymWorkoutPlanId 
        /// </summary>
        /// <param name="gymWorkoutPlanId">The ID of the gym Workout Plan Id.</param>
        /// <returns>Returns GymWorkoutPlanResponse.</returns>
        GymWorkoutPlanViewModel GetWorkoutPlanDetails(long gymWorkoutPlanId);

        /// <summary>
        /// Create WorkoutPlanDetails.
        /// </summary>
        /// <param name="gymWorkoutPlanDetailsViewModel">General AddWorkoutPlanDetails View Model.</param>
        /// <returns>Returns created model.</returns>
        GymWorkoutPlanDetailsViewModel AddWorkoutPlanDetails(GymWorkoutPlanDetailsViewModel gymWorkoutPlanDetailsViewModel);

        #region DeleteGymWorkoutPlanDetails

        /// <summary>
        /// Delete GymWorkoutPlanDetailsList.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteWorkoutPlanDetailsList(long gymWorkoutPlanId, out string errorMessage);

        /// <summary>
        /// Delete GymWorkoutPlanDetailsWeek.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanIds.</param>
        /// <param name="workoutWeekNumber">gymWorkoutPlanIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteWorkoutPlanDetailsWeek(long gymWorkoutPlanId, short workoutWeekNumber, out string errorMessage);

        /// <summary>
        /// Delete GymWorkoutPlanDetailsDay.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanIds.</param>
        /// <param name="workoutWeekNumber">gymWorkoutPlanIds.</param>
        /// <param name="workoutDayNumber">gymWorkoutPlanIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteWorkoutPlanDetailsDay(long gymWorkoutPlanId, short workoutWeekNumber, byte workoutDayNumber, out string errorMessage);

        /// <summary>
        /// Delete WorkoutPlanDetailsSet
        /// </summary>
        /// <param name="gymWorkoutPlanDetailId">gymWorkoutPlanIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteWorkoutPlanDetailsSet(long gymWorkoutPlanDetailId, out string errorMessage);

        #endregion

        #region Gym Workout Plan User
        /// <summary>
        /// Get list of Gym Workout Plan User.
        /// </summary>
        /// <param name="gymWorkoutPlanId">DataTable ViewModel.</param>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>GymWorkoutPlanUserViewModelListViewModel</returns>
        GymWorkoutPlanUserListViewModel GetAssociatedMemberList(long gymWorkoutPlanId, DataTableViewModel dataTableModel);

        /// <summary>
        /// Update Associate UnAssociate Workout Plan User.
        /// </summary>
        /// <param name="gymWorkoutPlanUserViewModel">gymWorkoutPlanUserViewModel.</param>
        /// <returns>Returns updated GymWorkoutPlanUserViewModel</returns>
        GymWorkoutPlanUserViewModel AssociateUnAssociateWorkoutPlanUser(GymWorkoutPlanUserViewModel gymWorkoutPlanUserViewModel);

        #endregion
    }
    #endregion
}
