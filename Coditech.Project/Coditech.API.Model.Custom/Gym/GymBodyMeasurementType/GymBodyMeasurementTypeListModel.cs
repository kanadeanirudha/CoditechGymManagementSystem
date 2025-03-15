namespace Coditech.Common.API.Model
{
    public class GymBodyMeasurementTypeListModel : BaseListModel
    {
        public List<GymBodyMeasurementTypeModel> GymBodyMeasurementTypeList { get; set; }
        public GymBodyMeasurementTypeListModel()
        {
            GymBodyMeasurementTypeList = new List<GymBodyMeasurementTypeModel>();
        }

    }
}