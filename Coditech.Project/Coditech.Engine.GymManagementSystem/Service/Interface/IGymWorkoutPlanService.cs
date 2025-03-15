using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymWorkoutPlanService
    {
        GymWorkoutPlanListModel GetGymWorkoutPlanList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymWorkoutPlanModel CreateGymWorkoutPlan(GymWorkoutPlanModel model);
        GymWorkoutPlanModel GetGymWorkoutPlan(long gymWorkoutPlanId);
        bool UpdateGymWorkoutPlan(GymWorkoutPlanModel model);       
        GymWorkoutPlanModel GetWorkoutPlanDetails(long gymWorkoutPlanId);
        GymWorkoutPlanDetailsModel AddWorkoutPlanDetails(GymWorkoutPlanDetailsModel model);
        bool DeleteGymWorkoutPlanDetails(DeleteWorkoutPlanDetailsModel parameterModel);
        GymWorkoutPlanUserListModel GetAssociatedMemberList(long gymWorkoutPlanId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        bool AssociateUnAssociateWorkoutPlanUser(GymWorkoutPlanUserModel model);
    }
}
