var GymMembershipPlan = {
    Initialize: function () {
        GymMembershipPlan.constructor();
    },
    constructor: function () {
    },

    ChangePlanType: function () {
        var planType = $("#PlanDurationTypeEnumId option:selected").text();
        if (planType == "Duration") {
            $("#PlanDurationInMonthDivId").show();
            $("#PlanDurationInDaysDivId").show();
            $("#PlanDurationInSessionDivId").hide();
        } else {
            $("#PlanDurationInMonthDivId").hide();
            $("#PlanDurationInDaysDivId").hide();
            $("#PlanDurationInSessionDivId").show();
        }
    },
    ChangeMaxCost: function () {
        $("#MinCost").val($("#MaxCost").val());
    }
}