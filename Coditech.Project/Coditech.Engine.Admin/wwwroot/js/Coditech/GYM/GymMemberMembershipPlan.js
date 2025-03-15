var GymMemberMembershipPlan = {
    Initialize: function () {
        GymMemberMembershipPlan.constructor();
    },
    constructor: function () {
    },

    ChangeMembershipPlan: function () {
        $("#errorGymMembershipPlanId").text('').text("").removeClass("field-validation-error").hide();
        var membershipPlan = $("#SelectedGymMembershipPlanId option:selected").val();
        $("#DiscountAmount").val("0");
        if (membershipPlan == '') {
            $("#GymMembershipPlanId").val("0");
            $("#PlanAmount").val("");
            $("#hdnMaximunDiscount").val("0");
        }
        else {
            var membershipPlanArray = membershipPlan.split("~");
            $("#GymMembershipPlanId").val(membershipPlanArray[0]);
            $("#PlanAmount").val(membershipPlanArray[2]);
            $("#hdnMaximunDiscount").val(membershipPlanArray[3]);
            if (parseInt(membershipPlanArray[3]) == 0) {
                $("#DiscountAmount").attr('disabled', 'disabled');
            }
            else {
                $("#DiscountAmount").removeAttr('disabled');
            }
        }

    },

    AssociateGymMemberMembershipPlan: function () {
        $("#AssociateGymMemberMembershipPlan").validate();
        $("#errorGymMembershipPlanId").text('').text("").removeClass("field-validation-error").hide();
        $("#errorPaymentTypeEnumId").text('').text("").removeClass("field-validation-error").hide();
        $("#errorTransactionReference").text('').text("").removeClass("field-validation-error").hide();
        $("#errorDiscountAmount").text('').text("").removeClass("field-validation-error").hide();

        var membershipPlan = $("#SelectedGymMembershipPlanId option:selected").val();
        if ($("#frmAssociateGymMemberMembershipPlan").valid()) {
            if (membershipPlan == "") {
                $("#errorGymMembershipPlanId").text('').text("Please Select Membership Plan").addClass("field-validation-error").show();
                return false;
            }
            if ($("#DiscountAmount").val() != "0" && parseInt($("#DiscountAmount").val()) > parseInt($("#hdnMaximunDiscount").val())) {
                $("#errorDiscountAmount").text('').text("Discount Amount should be less than " + parseInt($("#hdnMaximunDiscount").val())).addClass("field-validation-error").show();
                return false;
            }
            if ($("#PaymentTypeEnumId").val() == "") {
                $("#errorPaymentTypeEnumId").text('').text("Please select Mode Of Transaction").addClass("field-validation-error").show();
                return false;
            }
            if ($("#PaymentTypeEnumId option:selected").text() != "Cash" && $("#TransactionReference").val() == "") {
                $("#errorTransactionReference").text('').text("Please enter Transaction Reference").addClass("field-validation-error").show();
                return false;
            }

            $("#frmAssociateGymMemberMembershipPlan").submit();
        }
    },
}
