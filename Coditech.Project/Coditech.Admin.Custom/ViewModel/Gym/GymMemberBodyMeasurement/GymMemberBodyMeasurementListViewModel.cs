using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberBodyMeasurementListViewModel : BaseViewModel
    {
        public List<GymMemberBodyMeasurementViewModel> GymMemberBodyMeasurementList { get; set; }
        public GymMemberBodyMeasurementListViewModel()
        {
            GymMemberBodyMeasurementList = new List<GymMemberBodyMeasurementViewModel>();
        }
        public int GymMemberDetailId { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
