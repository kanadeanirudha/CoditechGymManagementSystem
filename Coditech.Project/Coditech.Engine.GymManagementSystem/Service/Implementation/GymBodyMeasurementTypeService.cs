
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;

using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class GymBodyMeasurementTypeService : IGymBodyMeasurementTypeService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GymBodyMeasurementType> _gymBodyMeasurementTypeRepository;
        public GymBodyMeasurementTypeService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _gymBodyMeasurementTypeRepository = new CoditechRepository<GymBodyMeasurementType>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual GymBodyMeasurementTypeListModel GetGymBodyMeasurementTypeList(FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GymBodyMeasurementTypeModel> objStoredProc = new CoditechViewRepository<GymBodyMeasurementTypeModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GymBodyMeasurementTypeModel> measurementTypeList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymBodyMeasurementTypeList @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 4, out pageListModel.TotalRowCount)?.ToList();
            GymBodyMeasurementTypeListModel listModel = new GymBodyMeasurementTypeListModel();

            listModel.GymBodyMeasurementTypeList = measurementTypeList?.Count > 0 ? measurementTypeList : new List<GymBodyMeasurementTypeModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }
        //Create Gym Body Measurement Type.
        public virtual GymBodyMeasurementTypeModel CreateGymBodyMeasurementType(GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel)
        {
            if (IsNull(gymBodyMeasurementTypeModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsBodyMeasurementTypeAlreadyExist(gymBodyMeasurementTypeModel.BodyMeasurementType))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Body Measurement Type"));

            GymBodyMeasurementType gymBodyMeasurementType = gymBodyMeasurementTypeModel.FromModelToEntity<GymBodyMeasurementType>();

            //Create new GymBodyMeasurementType and return it.
            GymBodyMeasurementType measurementTypeData = _gymBodyMeasurementTypeRepository.Insert(gymBodyMeasurementType);
            if (measurementTypeData?.GymBodyMeasurementTypeId > 0)
            {
                gymBodyMeasurementTypeModel.GymBodyMeasurementTypeId = measurementTypeData.GymBodyMeasurementTypeId;
            }
            else
            {
                gymBodyMeasurementTypeModel.HasError = true;
                gymBodyMeasurementTypeModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return gymBodyMeasurementTypeModel;
        }

        //Get GymBodyMeasurementType by gymBodyMeasurementType id.
        public virtual GymBodyMeasurementTypeModel GetGymBodyMeasurementType(short gymBodyMeasurementTypeId)
        {
            if (gymBodyMeasurementTypeId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymBodyMeasurementTypeID"));

            //Get the GymBodyMeasurementType Details based on id.
            GymBodyMeasurementType gymBodyMeasurementType = _gymBodyMeasurementTypeRepository.Table.FirstOrDefault(x => x.GymBodyMeasurementTypeId == gymBodyMeasurementTypeId);
            GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel = gymBodyMeasurementType?.FromEntityToModel<GymBodyMeasurementTypeModel>();
            return gymBodyMeasurementTypeModel;
        }

        //Update GymBodyMeasurementType.
        public virtual bool UpdateGymBodyMeasurementType(GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel)
        {
            if (IsNull(gymBodyMeasurementTypeModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (gymBodyMeasurementTypeModel.GymBodyMeasurementTypeId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymBodyMeasurementTypeId"));

            if (IsBodyMeasurementTypeAlreadyExist(gymBodyMeasurementTypeModel.BodyMeasurementType, gymBodyMeasurementTypeModel.GymBodyMeasurementTypeId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Body Measurement Type"));

            GymBodyMeasurementType gymBodyMeasurementType = gymBodyMeasurementTypeModel.FromModelToEntity<GymBodyMeasurementType>();

            //Update GymBodyMeasurementType
            bool isGymBodyMeasurementTypeUpdated = _gymBodyMeasurementTypeRepository.Update(gymBodyMeasurementType);
            if (!isGymBodyMeasurementTypeUpdated)
            {
                gymBodyMeasurementTypeModel.HasError = true;
                gymBodyMeasurementTypeModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isGymBodyMeasurementTypeUpdated;
        }

        //Delete GymBodyMeasurementType.
        public virtual bool DeleteGymBodyMeasurementType(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymBodyMeasurementTypeID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("GymBodyMeasurementTypeId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteGymBodyMeasurementType @GymBodyMeasurementTypeId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        //Check if Body Measurement Type is already present or not.
        protected virtual bool IsBodyMeasurementTypeAlreadyExist(string bodyMeasurementType, short gymBodyMeasurementTypeId = 0)
         => _gymBodyMeasurementTypeRepository.Table.Any(x => x.BodyMeasurementType == bodyMeasurementType && (x.GymBodyMeasurementTypeId != gymBodyMeasurementTypeId || gymBodyMeasurementTypeId == 0));
        #endregion
    }
}
