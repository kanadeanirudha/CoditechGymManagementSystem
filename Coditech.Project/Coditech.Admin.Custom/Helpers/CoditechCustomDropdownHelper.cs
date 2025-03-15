using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.Helpers
{
    public static class CoditechCustomDropdownHelper
    {
        public static DropdownViewModel GeneralDropdownList(DropdownViewModel dropdownViewModel)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();
            if (Equals(dropdownViewModel.DropdownType, DropdownCustomTypeEnum.GymMembershipPlan.ToString()))
            {
                GetGymMembershipPlanList(dropdownViewModel, dropdownList);
            }
            dropdownViewModel.DropdownList = dropdownList;
            return dropdownViewModel;
        }

        private static void GetGymMembershipPlanList(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            dropdownList.Add(new SelectListItem() { Text = "-------Select Membership Plan-------", Value = "" });

            if (!string.IsNullOrEmpty(dropdownViewModel.Parameter))
            {
                FilterCollection filters = new FilterCollection();
                filters.Add(FilterKeys.IsActive, ProcedureFilterOperators.Equals, "1");
                GymMembershipPlanListResponse response = new GymMembershipPlanClient().List(dropdownViewModel.Parameter, null, filters, null, 1, int.MaxValue);
                GymMembershipPlanListModel list = new GymMembershipPlanListModel() { GymMembershipPlanList = response.GymMembershipPlanList };
                foreach (var item in list?.GymMembershipPlanList)
                {
                    string planDuration = item.PlanDurationType.ToLower() == "duration" ? $"{item.PlanDurationInMonth} Month {item.PlanDurationInDays} Days" : $"{item.PlanDurationInSession} Session";
                    dropdownList.Add(new SelectListItem()
                    {
                        Text = $"{item.MembershipPlanName}-{item.PlanType}-{planDuration}",
                        Value = $"{item.GymMembershipPlanId.ToString()}~{item.PlanDurationType}~{item.MaxCost}~{(item.MaxCost - item.MinCost)}",
                        Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.GymMembershipPlanId)
                    });
                }
            }
        }

    }
}






