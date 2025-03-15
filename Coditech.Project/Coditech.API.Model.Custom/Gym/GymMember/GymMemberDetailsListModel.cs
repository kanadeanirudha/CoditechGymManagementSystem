namespace Coditech.Common.API.Model
{
    public class GymMemberDetailsListModel : BaseListModel
    {
        public List<GymMemberDetailsModel> GymMemberDetailsList { get; set; }
        public GymMemberDetailsListModel()
        {
            GymMemberDetailsList = new List<GymMemberDetailsModel>();
        }

    }
}
