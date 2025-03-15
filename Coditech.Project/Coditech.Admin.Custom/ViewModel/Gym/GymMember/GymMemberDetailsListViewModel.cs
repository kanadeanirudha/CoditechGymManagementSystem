using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberDetailsListViewModel : BaseViewModel
    {
        public List<GymMemberDetailsViewModel> GymMemberDetailsList { get; set; }
        public GymMemberDetailsListViewModel()
        {
            GymMemberDetailsList = new List<GymMemberDetailsViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
        public string ListType { get; set; }
        public int SelectedDepartmentId { get; set; }
    }
}
