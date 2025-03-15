using Coditech.Admin.ViewModel;
using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Resources;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Admin.Agents
{
    public class GymBodyMeasurementTypeAgent : BaseAgent, IGymBodyMeasurementTypeAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymBodyMeasurementTypeClient _gymBodyMeasurementTypeClient;
        #endregion

        #region Public Constructor
        public GymBodyMeasurementTypeAgent(ICoditechLogging coditechLogging, IGymBodyMeasurementTypeClient gymBodyMeasurementTypeClient)
        {
            _coditechLogging = coditechLogging;
            _gymBodyMeasurementTypeClient = GetClient<IGymBodyMeasurementTypeClient>(gymBodyMeasurementTypeClient);
        }
        #endregion

        #region Public Methods
        public virtual GymBodyMeasurementTypeListViewModel GetGymBodyMeasurementTypeList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("BodyMeasurementType", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("DisplayOrder", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("IsActive", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "BodyMeasurementType" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymBodyMeasurementTypeListResponse response = _gymBodyMeasurementTypeClient.List(null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymBodyMeasurementTypeListModel gymBodyMeasurementTypeList = new GymBodyMeasurementTypeListModel { GymBodyMeasurementTypeList = response?.GymBodyMeasurementTypeList };
            GymBodyMeasurementTypeListViewModel listViewModel = new GymBodyMeasurementTypeListViewModel();
            listViewModel.GymBodyMeasurementTypeList = gymBodyMeasurementTypeList?.GymBodyMeasurementTypeList?.ToViewModel<GymBodyMeasurementTypeViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymBodyMeasurementTypeList.Count, BindColumns());
            return listViewModel;
        }

        //Create Gym Body Measurement Type.
        public virtual GymBodyMeasurementTypeViewModel CreateGymBodyMeasurementType(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel)
        {
            try
            {
                GymBodyMeasurementTypeResponse response = _gymBodyMeasurementTypeClient.CreateGymBodyMeasurementType(gymBodyMeasurementTypeViewModel.ToModel<GymBodyMeasurementTypeModel>());
                GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel = response?.GymBodyMeasurementTypeModel;
                return IsNotNull(gymBodyMeasurementTypeModel) ? gymBodyMeasurementTypeModel.ToViewModel<GymBodyMeasurementTypeViewModel>() : new GymBodyMeasurementTypeViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymBodyMeasurementTypeViewModel)GetViewModelWithErrorMessage(gymBodyMeasurementTypeViewModel, ex.ErrorMessage);
                    default:
                        return (GymBodyMeasurementTypeViewModel)GetViewModelWithErrorMessage(gymBodyMeasurementTypeViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return (GymBodyMeasurementTypeViewModel)GetViewModelWithErrorMessage(gymBodyMeasurementTypeViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get GymBodyMeasurementType by gymBodyMeasurementType id.
        public virtual GymBodyMeasurementTypeViewModel GetGymBodyMeasurementType(short gymBodyMeasurementTypeId)
        {
            GymBodyMeasurementTypeResponse response = _gymBodyMeasurementTypeClient.GetGymBodyMeasurementType(gymBodyMeasurementTypeId);
            return response?.GymBodyMeasurementTypeModel.ToViewModel<GymBodyMeasurementTypeViewModel>();
        }

        //Update GymBodyMeasurementType.
        public virtual GymBodyMeasurementTypeViewModel UpdateGymBodyMeasurementType(GymBodyMeasurementTypeViewModel gymBodyMeasurementTypeViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymBodyMeasurementType", TraceLevel.Info);
                GymBodyMeasurementTypeResponse response = _gymBodyMeasurementTypeClient.UpdateGymBodyMeasurementType(gymBodyMeasurementTypeViewModel.ToModel<GymBodyMeasurementTypeModel>());
                GymBodyMeasurementTypeModel gymBodyMeasurementTypeModel = response?.GymBodyMeasurementTypeModel;
                _coditechLogging.LogMessage("Agent method execution done.", "GymBodyMeasurementType", TraceLevel.Info);
                return IsNotNull(gymBodyMeasurementTypeModel) ? gymBodyMeasurementTypeModel.ToViewModel<GymBodyMeasurementTypeViewModel>() : (GymBodyMeasurementTypeViewModel)GetViewModelWithErrorMessage(new GymBodyMeasurementTypeViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                return (GymBodyMeasurementTypeViewModel)GetViewModelWithErrorMessage(gymBodyMeasurementTypeViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete gymBodyMeasurementType.
        public virtual bool DeleteGymBodyMeasurementType(string gymBodyMeasurementTypeId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymBodyMeasurementType", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymBodyMeasurementTypeClient.DeleteGymBodyMeasurementType(new ParameterModel { Ids = gymBodyMeasurementTypeId });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymBodyMeasurementType;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymBodyMeasurementType", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Body Measurement Type",
                ColumnCode = "BodyMeasurementType",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Measurement Unit",
                ColumnCode = "MeasurementUnitDisplayName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Display Order",
                ColumnCode = "DisplayOrder",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Active",
                ColumnCode = "IsActive",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
        #region
        // it will return get all GymBodyMeasurementType list from database 
        public virtual GymBodyMeasurementTypeListResponse GetGymBodyMeasurementTypeList()
        {
            GymBodyMeasurementTypeListResponse gymBodyMeasurementTypeList = _gymBodyMeasurementTypeClient.List(null, null, null, 1, int.MaxValue);
            return gymBodyMeasurementTypeList?.GymBodyMeasurementTypeList?.Count > 0 ? gymBodyMeasurementTypeList : new GymBodyMeasurementTypeListResponse();
        }
        #endregion
    }
}
