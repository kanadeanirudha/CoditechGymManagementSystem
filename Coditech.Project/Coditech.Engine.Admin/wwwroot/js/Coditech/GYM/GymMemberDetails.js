var GymMemberDetails = {
    Initialize: function () {
        GymMemberDetails.constructor();
    },
    constructor: function () {
    },

    GetMemberFollowUp: function (modelPopContentId, gymMemberDetailId, gymMemberFollowUpId, personId) {
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GymMemberDetails/GetMemberFollowUp",
                data: { "gymMemberDetailId": gymMemberDetailId, "gymMemberFollowUpId": gymMemberFollowUpId, "personId": personId },
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#' + modelPopContentId).html("").html(result);
                    CoditechCommon.HideLodder();
                },
                error: function (xhr, ajaxOptions, thrownError) {                    
                    if (xhr.status == "401" || xhr.status == "403") {
                        location.reload();
                    }
                    CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                    CoditechCommon.HideLodder();
                }
            });
    },
    SaveFollowup: function () {
        $("#frmGymMemberFollowUp").validate();
        $("#errorGymFollowupTypesEnumId").text('').text("").removeClass("field-validation-error").hide();
        $("#errorReminderDate").text('').text("").removeClass("field-validation-error").hide();
        if ($("#frmGymMemberFollowUp").valid()) {
            if ($("#GymFollowupTypesEnumId").val() == "" || $("#GymFollowupTypesEnumId").val().includes("select") || $("#GymFollowupTypesEnumId").val().includes("Select")) {
                $("#errorGymFollowupTypesEnumId").text('').text("Please Select Follow-up Type.").addClass("field-validation-error").show();
                return false;
            }
            if ($("#IsSetReminder").is(':checked') && $("#ReminderDate").val() == "") {
                $("#errorReminderDate").text('').text("Please Select Reminder Date.").addClass("field-validation-error").show();
                return false;
            }
            $("#frmGymMemberFollowUp").submit();
        }
    },

    CreatEditGeneralPersonAttendanceDetails: function (modelPopContentId, gymMemberDetailId, generalPersonAttendanceDetailId) {
        console.log(modelPopContentId, gymMemberDetailId, generalPersonAttendanceDetailId)

        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            url: "/GymMemberDetails/GetGeneralPersonAttendanceDetails",
            data: { "gymMemberDetailId": gymMemberDetailId, "generalPersonAttendanceDetailId": generalPersonAttendanceDetailId },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html("").html(result);
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to display manual attendance.", "error");
                CoditechCommon.HideLodder();
            }
        });
    },

    SaveManualAttendance: function () {
        $("#frmGymMemberManualAttendance").validate();
        $("#errorGeneralAttendanceStateEnumId").text('').removeClass("field-validation-error").hide();
        $("#errorAttendancedate").text('').removeClass("field-validation-error").hide();
        $("#errorLoginTime").text('').removeClass("field-validation-error").hide();
        $("#errorLogoutTime").text('').removeClass("field-validation-error").hide();

        var selectedOption = $("#GeneralAttendanceStateEnumId").val();

        if (!selectedOption) {
            $("#errorGeneralAttendanceStateEnumId").text("Please Select Attendance State.").addClass("field-validation-error").show();
            return false;
        }

        if ($("#frmGymMemberManualAttendance").valid()) {
            // Check LogoutTime field using showLogoutTimeField logic
            var loginTimeValue = $("#LoginTime").val();
            var logoutTimeValue = $("#LogoutTime").val();

            if (loginTimeValue === '' && (selectedOption === 'CheckIn' || selectedOption === 'Both')) {
                $("#errorLoginTime").text("Please enter Check-In Time.").addClass("field-validation-error").show();
                return false;
            }

            if (logoutTimeValue === '' && (selectedOption === 'CheckOut' || selectedOption === 'Both')) {
                $("#errorLogoutTime").text("Please enter Check-Out Time.").addClass("field-validation-error").show();
                return false;
            }

            if (logoutTimeValue !== '' && loginTimeValue >= logoutTimeValue) {
                $("#errorLogoutTime").text("Check-Out Time must be greater than Check-In Time.").addClass("field-validation-error").show();
                return false;
            }

            $("#frmGymMemberManualAttendance").submit();
        }
    },

    DisplayCheckInCheckOutDiv: function () {
        var selectedOption = $("#GeneralAttendanceStateEnumId").val();
        $("#errorGeneralAttendanceStateEnumId").text('').removeClass("field-validation-error").hide();
        $("#errorLoginTime").text('').removeClass("field-validation-error").hide();
        $("#errorLogoutTime").text('').removeClass("field-validation-error").hide();

        var showLoginField = function () {
            $("#LoginTimeDivId").show();
        };

        var hideLoginField = function () {
            $("#LoginTimeDivId").hide();
        };

        var showLogoutField = function () {
            $("#LogoutTimeDivId").show();
        };

        var hideLogoutField = function () {
            $("#LogoutTimeDivId").hide();
        };

        switch (selectedOption) {
            case "CheckIn":
                showLoginField();
                hideLogoutField();
                break;
            case "CheckOut":
                hideLoginField();
                showLogoutField();
                break;
            case "Both":
                showLoginField();
                showLogoutField();
                break;
            default:
                // Handle other cases or set a default behavior if needed
                hideLoginField();
                hideLogoutField();
                break;
        }
    },

    IsSetReminder: function () {
        $("#ReminderDate").val("");
        if ($("#IsSetReminder").is(':checked')) {
            // Code in the case checkbox is checked.
            $("#ReminderDateDivId").show();
        } else {
            // Code in the case checkbox is NOT checked.
            $("#ReminderDateDivId").hide();
        }
    },

    AddGymMemberBodyMeasurement: function (modelPopContentId, gymMemberDetailId, gymBodyMeasurementTypeId, gymMemberBodyMeasurementId, bodyMeasurementType, measurementUnitShortCode, personId) {
        CoditechCommon.ShowLodder();
        $.ajax({
            cache: false,
            type: "GET",
            dataType: "html",
            async: false,
            url: "/GymMemberDetails/GetGymMemberBodyMeasurement",
            data: {
                gymMemberDetailId: gymMemberDetailId,
                gymMemberBodyMeasurementId: gymMemberBodyMeasurementId,
                gymBodyMeasurementTypeId: gymBodyMeasurementTypeId,
                bodyMeasurementType: bodyMeasurementType,
                measurementUnitShortCode: measurementUnitShortCode,
                personId: personId
            },
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('#' + modelPopContentId).html("").html(result);
                CoditechCommon.HideLodder();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status == "401" || xhr.status == "403") {
                    location.reload();
                }
                CoditechNotification.DisplayNotificationMessage("Failed to display record.", "error");
                CoditechCommon.HideLodder();
            }
        });
    }




}
