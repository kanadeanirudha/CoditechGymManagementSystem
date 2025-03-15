namespace Coditech.Common.API.Model
{
    public class GymMemberFollowUpListModel : BaseListModel
    {
        public List<GymMemberFollowUpModel> GymMemberFollowUpList { get; set; }
        public GymMemberFollowUpListModel()
        {
            GymMemberFollowUpList = new List<GymMemberFollowUpModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
