using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymMembershipPlanService
    {
        GymMembershipPlanListModel GetGymMembershipPlanList(string selectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymMembershipPlanModel CreateGymMembershipPlan(GymMembershipPlanModel model);
        GymMembershipPlanModel GetGymMembershipPlan(int gymMemberDetailId);
        bool UpdateGymMembershipPlan(GymMembershipPlanModel model);
        bool DeleteGymMembershipPlan(ParameterModel parameterModel);
    }
}
