using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberFollowUpListViewModel : BaseViewModel
    {
        public List<GymMemberFollowUpViewModel> GymMemberFollowUpList { get; set; }
        public GymMemberFollowUpListViewModel()
        {
            GymMemberFollowUpList = new List<GymMemberFollowUpViewModel>();
        }
        public long PersonId { get; set; }
        public int GymMemberDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
    }
}
