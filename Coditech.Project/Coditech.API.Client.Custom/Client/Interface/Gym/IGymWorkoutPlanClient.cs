using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IGymWorkoutPlanClient : IBaseClient
    {
        /// <summary>
        /// Get list of GymWorkoutPlan.
        /// </summary>
        /// <returns>GymWorkoutPlanListResponse</returns>
        GymWorkoutPlanListResponse List(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create GymWorkoutPlan.
        /// </summary>
        /// <param name="GymWorkoutPlanModel">GymWorkoutPlanModel.</param>
        /// <returns>Returns GymWorkoutPlanResponse.</returns>
        GymWorkoutPlanResponse CreateGymWorkoutPlan(GymWorkoutPlanModel body);

        /// <summary>
        /// Get GymWorkoutPlan by gymWorkoutPlanId.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanId</param>
        /// <returns>Returns GymWorkoutPlanResponse.</returns>
        GymWorkoutPlanResponse GetGymWorkoutPlan(long gymWorkoutPlanId);

        /// <summary>
        /// Update Gym Workout Plan.
        /// </summary>
        /// <param name="GymWorkoutPlanModel">GymWorkoutPlanModel.</param>
        /// <returns>Returns updated GymWorkoutPlanResponse</returns>
        GymWorkoutPlanResponse UpdateGymWorkoutPlan(GymWorkoutPlanModel body);

        /// <summary>
        /// Get WorkoutPlan by gymWorkoutPlanId.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanId</param>
        /// <returns>Returns WorkoutPlanResponse.</returns>
        GymWorkoutPlanResponse GetWorkoutPlanDetails(long gymWorkoutPlanId);

        /// <summary>
        /// Create WorkoutPlanDetails.
        /// </summary>
        /// <param name="GymWorkoutPlanDetailsModel">GymWorkoutPlanDetailsModel.</param>
        /// <returns>Returns GymWorkoutPlanDetailsResponse.</returns>
        GymWorkoutPlanDetailsResponse AddWorkoutPlanDetails(GymWorkoutPlanDetailsModel body);

        #region Delete
        /// <summary>
        /// Delete GymWorkoutPlan.
        /// </summary>
        /// <param name="DeleteWorkoutPlanDetailsModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteGymWorkoutPlanDetails(DeleteWorkoutPlanDetailsModel body);

        #region Gym Workout Plan User
        /// <summary>
        /// Get list of Gym Workout Plan User.
        /// </summary>
        /// <param name="gymWorkoutPlanId">gymWorkoutPlanId</param>
        /// <returns>GymWorkoutPlanUserListResponse</returns>
        GymWorkoutPlanUserListResponse GetAssociatedMemberList(long gymWorkoutPlanId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Update Associate UnAssociate Gym Workout Plan User.
        /// </summary>
        /// <param name="GymWorkoutPlanUserModel">GymWorkoutPlanUserModel.</param>
        /// <returns>Returns updated GymWorkoutPlanUserResponse</returns>
        GymWorkoutPlanUserResponse AssociateUnAssociateWorkoutPlanUser(GymWorkoutPlanUserModel body);
        #endregion

        #endregion

    }
}
