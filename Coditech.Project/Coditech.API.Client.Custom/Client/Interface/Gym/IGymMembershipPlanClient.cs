using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IGymMembershipPlanClient : IBaseClient
    {
        /// <summary>
        /// Get list of Gym Member.
        /// </summary>
        /// <returns>GymMembershipPlanListResponse</returns>
        GymMembershipPlanListResponse List(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create Gym Membership Plan
        /// </summary>
        /// <param name="GymMembershipPlanModel">GymMembershipPlanModel.</param>
        /// <returns>Returns GymMembershipPlanResponse.</returns>
        GymMembershipPlanResponse CreateGymMembershipPlan(GymMembershipPlanModel body);

        /// <summary>
        /// Get Gym Membership Plan by gymMembershipPlanId.
        /// </summary>
        /// <param name="gymMembershipPlanId">gymMembershipPlanId</param>
        /// <returns>Returns GymMembershipPlanResponse.</returns>
        GymMembershipPlanResponse GetGymMembershipPlan(int gymMembershipPlanId);

        /// <summary>
        /// Update Gym Membership Plan.
        /// </summary>
        /// <param name="GymMembershipPlanModel">GymMembershipPlanModel.</param>
        /// <returns>Returns updated GymMembershipPlanResponse</returns>
        GymMembershipPlanResponse UpdateGymMembershipPlan(GymMembershipPlanModel body);

        /// <summary>
        /// Delete Gym Membership plan.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteGymMembershipPlan(ParameterModel body);
    }
}
