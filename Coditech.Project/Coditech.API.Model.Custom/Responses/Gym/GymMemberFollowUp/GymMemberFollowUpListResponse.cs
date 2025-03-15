namespace Coditech.Common.API.Model.Response
{
    public class GymMemberFollowUpListResponse : BaseListResponse
    {
        public List<GymMemberFollowUpModel> GymMemberFollowUpList { get; set; }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
