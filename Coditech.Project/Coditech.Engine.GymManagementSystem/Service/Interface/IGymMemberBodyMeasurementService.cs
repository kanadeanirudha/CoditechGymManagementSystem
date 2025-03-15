using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymMemberBodyMeasurementService
    {
        GymMemberBodyMeasurementListModel GetBodyMeasurementTypeListByMemberId(int gymMemberDetailId, long personId, short pageSize);
        GymMemberBodyMeasurementListModel GetMemberBodyMeasurementList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymMemberBodyMeasurementModel CreateMemberBodyMeasurement(GymMemberBodyMeasurementModel model);
        GymMemberBodyMeasurementModel GetMemberBodyMeasurement(long GymMemberBodyMeasurementId);
        bool UpdateMemberBodyMeasurement(GymMemberBodyMeasurementModel model);
        bool DeleteMemberBodyMeasurement(ParameterModel parameterModel);
    }
}

