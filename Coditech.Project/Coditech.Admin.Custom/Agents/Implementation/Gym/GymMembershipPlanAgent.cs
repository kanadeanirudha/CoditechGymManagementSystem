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
    public class GymMembershipPlanAgent : BaseAgent, IGymMembershipPlanAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymMembershipPlanClient _gymMembershipPlanClient;
        private readonly IInventoryGeneralItemMasterClient _inventoryGeneralItemMasterClient;
        #endregion

        #region Public Constructor
        public GymMembershipPlanAgent(ICoditechLogging coditechLogging, IGymMembershipPlanClient gymMembershipPlanClient, IUserClient userClient, IInventoryGeneralItemMasterClient inventoryGeneralItemMasterClient)
        {
            _coditechLogging = coditechLogging;
            _gymMembershipPlanClient = GetClient<IGymMembershipPlanClient>(gymMembershipPlanClient);
            _inventoryGeneralItemMasterClient = GetClient<IInventoryGeneralItemMasterClient>(inventoryGeneralItemMasterClient);
        }
        #endregion

        #region Public Methods
        public virtual GymMembershipPlanListViewModel GetGymMembershipPlanList(DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("MembershipPlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MaxCost", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MinCost", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "MembershipPlanName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymMembershipPlanListResponse response = _gymMembershipPlanClient.List(dataTableModel.SelectedCentreCode, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMembershipPlanListModel gymMemberList = new GymMembershipPlanListModel { GymMembershipPlanList = response?.GymMembershipPlanList };
            GymMembershipPlanListViewModel listViewModel = new GymMembershipPlanListViewModel();
            listViewModel.GymMembershipPlanList = gymMemberList?.GymMembershipPlanList?.ToViewModel<GymMembershipPlanViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMembershipPlanList.Count, BindColumns());
            return listViewModel;
        }

        #region General Person
        //Create Membership Plan
        public virtual GymMembershipPlanViewModel CreateGymMembershipPlan(GymMembershipPlanViewModel gymMembershipPlanViewModel)
        {
            try
            {
                GymMembershipPlanResponse response = _gymMembershipPlanClient.CreateGymMembershipPlan(gymMembershipPlanViewModel.ToModel<GymMembershipPlanModel>());
                GymMembershipPlanModel gymMembershipPlanModel = response?.GymMembershipPlanModel;
                return IsNotNull(gymMembershipPlanModel) ? gymMembershipPlanModel.ToViewModel<GymMembershipPlanViewModel>() : new GymMembershipPlanViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymMembershipPlanViewModel)GetViewModelWithErrorMessage(gymMembershipPlanViewModel, ex.ErrorMessage);
                    default:
                        return (GymMembershipPlanViewModel)GetViewModelWithErrorMessage(gymMembershipPlanViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymMembershipPlanViewModel)GetViewModelWithErrorMessage(gymMembershipPlanViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get Gym Membership Plan
        public virtual GymMembershipPlanViewModel GetGymMembershipPlan(int gymMembershipPlanId)
        {
            GymMembershipPlanResponse response = _gymMembershipPlanClient.GetGymMembershipPlan(gymMembershipPlanId);
            GymMembershipPlanViewModel gymMembershipPlanViewModel = response?.GymMembershipPlanModel.ToViewModel<GymMembershipPlanViewModel>();
            return gymMembershipPlanViewModel;
        }

        //Update Gym Member Other shipPlan.
        public virtual GymMembershipPlanViewModel UpdateGymMembershipPlan(GymMembershipPlanViewModel gymMembershipPlanViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                GymMembershipPlanResponse response = _gymMembershipPlanClient.UpdateGymMembershipPlan(gymMembershipPlanViewModel.ToModel<GymMembershipPlanModel>());
                GymMembershipPlanModel gymMembershipPlanModel = response?.GymMembershipPlanModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(gymMembershipPlanModel) ? gymMembershipPlanModel.ToViewModel<GymMembershipPlanViewModel>() : (GymMembershipPlanViewModel)GetViewModelWithErrorMessage(new GymMembershipPlanViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymMembershipPlanViewModel)GetViewModelWithErrorMessage(gymMembershipPlanViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        //Delete gym Membership Plan.
        public virtual bool DeleteGymMembershipPlan(string gymMembershipPlanIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymMembershipPlanClient.DeleteGymMembershipPlan(new ParameterModel { Ids = gymMembershipPlanIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymMembershipPlan;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        public virtual List<InventoryGeneralItemMasterModel> AllGeneralServices()
        {
            return _inventoryGeneralItemMasterClient.GetGeneralServicesList(string.Empty)?.InventoryGeneralItemMasterList;
        }
        #endregion

        #endregion

        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Membership Plan",
                ColumnCode = "MembershipPlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Type",
                ColumnCode = "PlanTypeEnumId",
                IsSortable = true,
            });

            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Membership Period",
                ColumnCode = "PlanDurationInMonthAndSession",
            });

            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Max Cost",
                ColumnCode = "MaxCost",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Min Cost",
                ColumnCode = "MinCost",
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

        protected virtual List<DatatableColumns> BindGymMemberFollowUpColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Comments",
                ColumnCode = "FollowupComment",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Follow-Up Type",
                ColumnCode = "FollowupType",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Set Reminder",
                ColumnCode = "IsSetReminder",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Reminder Date",
                ColumnCode = "ReminderDate",
            });
            return datatableColumnList;
        }
        #endregion
    }
}
