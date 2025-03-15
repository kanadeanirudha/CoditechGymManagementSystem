using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymBodyMeasurementTypeService
    {
        GymBodyMeasurementTypeListModel GetGymBodyMeasurementTypeList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymBodyMeasurementTypeModel CreateGymBodyMeasurementType(GymBodyMeasurementTypeModel model);
        GymBodyMeasurementTypeModel GetGymBodyMeasurementType(short gymBodyMeasurementTypeId);
        bool UpdateGymBodyMeasurementType(GymBodyMeasurementTypeModel model);
        bool DeleteGymBodyMeasurementType(ParameterModel parameterModel);
    }
}
