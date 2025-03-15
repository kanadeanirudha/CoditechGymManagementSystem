namespace Coditech.Common.API.Model
{
    public class GymUpcomingPlanExpirationMembersModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string MembershipPlanName { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDurationExpirationDate { get; set; }
        public int GymMemberDetailId { get; set; }
        public long PersonId { get; set; }
        public int PlanDurationExpiredInDays { get; set; }
    }
}
