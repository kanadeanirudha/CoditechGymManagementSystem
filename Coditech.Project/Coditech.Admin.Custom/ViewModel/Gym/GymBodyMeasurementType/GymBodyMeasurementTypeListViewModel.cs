using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymBodyMeasurementTypeListViewModel : BaseViewModel
    {
        public List<GymBodyMeasurementTypeViewModel> GymBodyMeasurementTypeList { get; set; }
        public GymBodyMeasurementTypeListViewModel()
        {
            GymBodyMeasurementTypeList = new List<GymBodyMeasurementTypeViewModel>();
        }
    }
}
