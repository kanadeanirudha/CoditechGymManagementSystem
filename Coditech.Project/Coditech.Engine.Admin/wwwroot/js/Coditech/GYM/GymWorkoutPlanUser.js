var GymWorkoutPlan = {
    Initialize: function () {
        GymWorkoutPlan.constructor();
    },
    constructor: function () {
    },

    GetAssociateUnAssociateWorkoutPlanUser: function (modelPopContentId, gymWorkoutPlanUserId, gymWorkoutPlanId, workoutPlanName, firstName, lastName, gymMemberDetailId) {

        let gymWorkoutPlanUserViewModel = {
            GymWorkoutPlanUserId: gymWorkoutPlanUserId,
            GymWorkoutPlanId: gymWorkoutPlanId,
            WorkoutPlanName: workoutPlanName,
            FirstName: firstName,
            LastName: lastName,
            GymMemberDetailId: gymMemberDetailId
            
        };
        CoditechCommon.ShowLodder();
        $.ajax(
            {
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GymWorkoutPlan/GetAssociateUnAssociateWorkoutPlanUser",
                data: gymWorkoutPlanUserViewModel,
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
    AssociateUnAssociateWorkoutPlanUser: function () {
        $("#frmAssociateUnAssociateWorkoutPlanUser").submit();
    },
}
