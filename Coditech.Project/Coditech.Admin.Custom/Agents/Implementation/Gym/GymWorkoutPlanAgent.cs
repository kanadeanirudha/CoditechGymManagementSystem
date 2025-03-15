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

using Newtonsoft.Json;
using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.Admin.Agents
{
    public class GymWorkoutPlanAgent : BaseAgent, IGymWorkoutPlanAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymWorkoutPlanClient _gymWorkoutPlanClient;
        private readonly IGymWorkoutPlanClient _gymWorkoutPlanDetailsClient;
        private readonly IGymWorkoutPlanClient _gymWorkoutPlanSetClient;
        private readonly IUserClient _userClient;
        #endregion

        #region Public Constructor
        public GymWorkoutPlanAgent(ICoditechLogging coditechLogging, IGymWorkoutPlanClient gymWorkoutPlanClient, IGymWorkoutPlanClient gymWorkoutPlanDetailsClient, IGymWorkoutPlanClient gymWorkoutPlanSetClient, IUserClient userClient)
        {
            _coditechLogging = coditechLogging;
            _gymWorkoutPlanClient = GetClient<IGymWorkoutPlanClient>(gymWorkoutPlanClient);
            _gymWorkoutPlanDetailsClient = GetClient<IGymWorkoutPlanClient>(gymWorkoutPlanClient);
            _gymWorkoutPlanSetClient = GetClient<IGymWorkoutPlanClient>(gymWorkoutPlanClient);
            _userClient = GetClient<IUserClient>(userClient);
        }
        #endregion

        #region Public Methods
        public virtual GymWorkoutPlanListViewModel GetGymWorkoutPlanList(DataTableViewModel dataTableModel, string listType = null)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();

            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("WorkoutPlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("Target", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("NumberOfWeeks", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("NumberOfDaysPerWeek", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            filters.Add(FilterKeys.SelectedCentreCode, ProcedureFilterOperators.Equals, dataTableModel.SelectedCentreCode);

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymWorkoutPlanListResponse response = _gymWorkoutPlanClient.List(dataTableModel.SelectedCentreCode, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymWorkoutPlanListModel gymWorkoutPlanList = new GymWorkoutPlanListModel { GymWorkoutPlanList = response?.GymWorkoutPlanList };
            GymWorkoutPlanListViewModel listViewModel = new GymWorkoutPlanListViewModel();
            listViewModel.GymWorkoutPlanList = gymWorkoutPlanList?.GymWorkoutPlanList?.ToViewModel<GymWorkoutPlanViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymWorkoutPlanList.Count, BindColumns());
            return listViewModel;
        }

        #region Gym Workout Plan
        //Create Gym Workout Plan .
        public virtual GymWorkoutPlanViewModel CreateGymWorkoutPlan(GymWorkoutPlanViewModel gymWorkoutPlanViewModel)
        {
            try
            {
                GymWorkoutPlanResponse response = _gymWorkoutPlanClient.CreateGymWorkoutPlan(gymWorkoutPlanViewModel.ToModel<GymWorkoutPlanModel>());
                GymWorkoutPlanModel gymWorkoutPlanModel = response?.GymWorkoutPlanModel;
                return IsNotNull(gymWorkoutPlanModel) ? gymWorkoutPlanModel.ToViewModel<GymWorkoutPlanViewModel>() : new GymWorkoutPlanViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymWorkoutPlanViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanViewModel, ex.ErrorMessage);
                    default:
                        return (GymWorkoutPlanViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return (GymWorkoutPlanViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }
        //Get  Gym Workout Plan
        public virtual GymWorkoutPlanViewModel GetGymWorkoutPlan(long gymWorkoutPlanId)
        {
            GymWorkoutPlanResponse response = _gymWorkoutPlanClient.GetGymWorkoutPlan(gymWorkoutPlanId);
            GymWorkoutPlanViewModel gymWorkoutPlanViewModel = response?.GymWorkoutPlanModel.ToViewModel<GymWorkoutPlanViewModel>();
            return gymWorkoutPlanViewModel;
        }

        //Update Gym Workout Plan.
        public virtual GymWorkoutPlanViewModel UpdateGymWorkoutPlan(GymWorkoutPlanViewModel gymWorkoutPlanViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymWorkoutPlan", TraceLevel.Info);
                GymWorkoutPlanResponse response = _gymWorkoutPlanClient.UpdateGymWorkoutPlan(gymWorkoutPlanViewModel.ToModel<GymWorkoutPlanModel>());
                GymWorkoutPlanModel gymWorkoutPlanModel = response?.GymWorkoutPlanModel;
                _coditechLogging.LogMessage("Agent method execution done.", "GymWorkoutPlan", TraceLevel.Info);
                return IsNotNull(gymWorkoutPlanModel) ? gymWorkoutPlanModel.ToViewModel<GymWorkoutPlanViewModel>() : (GymWorkoutPlanViewModel)GetViewModelWithErrorMessage(new GymWorkoutPlanViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return (GymWorkoutPlanViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion

        #region Gym Workout Plan Details
        public virtual GymWorkoutPlanViewModel GetWorkoutPlanDetails(long gymWorkoutPlanId)
        {
            GymWorkoutPlanResponse response = _gymWorkoutPlanClient.GetWorkoutPlanDetails(gymWorkoutPlanId);
            GymWorkoutPlanViewModel gymWorkoutPlanViewModel = response?.GymWorkoutPlanModel.ToViewModel<GymWorkoutPlanViewModel>();
            return gymWorkoutPlanViewModel;
        }

        //Create Gym Workout Plan Details.
        public virtual GymWorkoutPlanDetailsViewModel AddWorkoutPlanDetails(GymWorkoutPlanDetailsViewModel gymWorkoutPlanDetailsViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(gymWorkoutPlanDetailsViewModel.GymWorkoutPlanData))
                {
                    List<GymWorkoutPlanSetModel> gymWorkoutPlanSetList = JsonConvert.DeserializeObject<List<GymWorkoutPlanSetModel>>(gymWorkoutPlanDetailsViewModel.GymWorkoutPlanData);
                    gymWorkoutPlanDetailsViewModel.GymWorkoutPlanSetList = gymWorkoutPlanSetList;
                }

                GymWorkoutPlanDetailsResponse response = _gymWorkoutPlanDetailsClient.AddWorkoutPlanDetails(gymWorkoutPlanDetailsViewModel.ToModel<GymWorkoutPlanDetailsModel>());
                GymWorkoutPlanDetailsModel gymWorkoutPlanDetailsModel = response?.GymWorkoutPlanDetailsModel;
                return IsNotNull(gymWorkoutPlanDetailsModel) ? gymWorkoutPlanDetailsModel.ToViewModel<GymWorkoutPlanDetailsViewModel>() : new GymWorkoutPlanDetailsViewModel();


            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymWorkoutPlanDetailsViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanDetailsViewModel, ex.ErrorMessage);
                    default:
                        return (GymWorkoutPlanDetailsViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanDetailsViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return (GymWorkoutPlanDetailsViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanDetailsViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        #endregion

        #region Delete Workout Plan Details      
        public virtual bool DeleteWorkoutPlanDetailsList(long gymWorkoutPlanId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymWorkoutPlan", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymWorkoutPlanClient.DeleteGymWorkoutPlanDetails(new DeleteWorkoutPlanDetailsModel { GymWorkoutplanDeleteType = "WorkoutPlan", GymWorkoutPlanId = gymWorkoutPlanId });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymWorkoutPlanDetails;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        //DeleteWorkoutPlanDetailsWeek
        public virtual bool DeleteWorkoutPlanDetailsWeek(long gymWorkoutPlanId, short workoutWeekNumber, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymWorkoutPlan", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymWorkoutPlanClient.DeleteGymWorkoutPlanDetails(new DeleteWorkoutPlanDetailsModel { GymWorkoutplanDeleteType = "WorkoutWeek", GymWorkoutPlanId = gymWorkoutPlanId, WorkoutWeekNumber = workoutWeekNumber });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymWorkoutPlanDetails;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        //DeleteWorkoutPlanDetailsDay
        public virtual bool DeleteWorkoutPlanDetailsDay(long gymWorkoutPlanId, short workoutWeekNumber, byte workoutDayNumber, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymWorkoutPlan", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymWorkoutPlanClient.DeleteGymWorkoutPlanDetails(new DeleteWorkoutPlanDetailsModel { GymWorkoutplanDeleteType = "WorkoutDay", GymWorkoutPlanId = gymWorkoutPlanId, WorkoutWeekNumber = workoutWeekNumber, WorkoutDayNumber = workoutDayNumber });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymWorkoutPlanDetails;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        //Delete  Gym Workout Plan Set.
        public virtual bool DeleteWorkoutPlanDetailsSet(long gymWorkoutPlanDetailId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "GymWorkoutPlan", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymWorkoutPlanClient.DeleteGymWorkoutPlanDetails(new DeleteWorkoutPlanDetailsModel { GymWorkoutplanDeleteType = "WorkoutSet", GymWorkoutPlandetailId = gymWorkoutPlanDetailId });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymWorkoutPlan;
                        return false;
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
        #endregion

        #region Gym Workout Plan User
        public virtual GymWorkoutPlanUserListViewModel GetAssociatedMemberList(long gymWorkoutPlanId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = new FilterCollection();
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }           

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymWorkoutPlanUserListResponse response = _gymWorkoutPlanClient.GetAssociatedMemberList(gymWorkoutPlanId,null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymWorkoutPlanUserListModel gymWorkoutPlanUserList = new GymWorkoutPlanUserListModel { GymWorkoutPlanUserList = response?.GymWorkoutPlanUserList };
            GymWorkoutPlanUserListViewModel listViewModel = new GymWorkoutPlanUserListViewModel();
            listViewModel.GymWorkoutPlanUserList = gymWorkoutPlanUserList?.GymWorkoutPlanUserList?.ToViewModel<GymWorkoutPlanUserViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymWorkoutPlanUserList.Count, BindAssociateWorkoutPlanUserColumns());
            listViewModel.GymWorkoutPlanId = gymWorkoutPlanId;
            listViewModel.WorkoutPlanName = response.WorkoutPlanName;
            return listViewModel;
        }    

        //Update Associate UnAssociate Centrewise Department.
        public virtual GymWorkoutPlanUserViewModel AssociateUnAssociateWorkoutPlanUser(GymWorkoutPlanUserViewModel gymWorkoutPlanUserViewModel)
        {
            try
            {
                long gymWorkoutPlanId = gymWorkoutPlanUserViewModel.GymWorkoutPlanId;
                long gymWorkoutPlanUserId = gymWorkoutPlanUserViewModel.GymWorkoutPlanUserId;
                GymWorkoutPlanUserResponse response = _gymWorkoutPlanClient.AssociateUnAssociateWorkoutPlanUser(gymWorkoutPlanUserViewModel.ToModel<GymWorkoutPlanUserModel>());
                GymWorkoutPlanUserModel gymWorkoutPlanUserModel = response?.GymWorkoutPlanUserModel;
                gymWorkoutPlanUserViewModel = IsNotNull(gymWorkoutPlanUserModel) ? gymWorkoutPlanUserModel.ToViewModel<GymWorkoutPlanUserViewModel>() : new GymWorkoutPlanUserViewModel();
                gymWorkoutPlanUserViewModel.GymWorkoutPlanId = gymWorkoutPlanId;
                gymWorkoutPlanUserViewModel.GymWorkoutPlanUserId = gymWorkoutPlanUserId;
                return gymWorkoutPlanUserViewModel;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymWorkoutPlanUserViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanUserViewModel, ex.ErrorMessage);
                    default:
                        return (GymWorkoutPlanUserViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanUserViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "GymWorkoutPlan", TraceLevel.Error);
                return (GymWorkoutPlanUserViewModel)GetViewModelWithErrorMessage(gymWorkoutPlanUserViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion
        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Workout Plan Name",
                ColumnCode = "WorkoutPlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Target",
                ColumnCode = "Target",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Number Of Weeks",
                ColumnCode = "NumberOfWeeks",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Number Of Days Per Week",
                ColumnCode = "NumberOfDaysPerWeek",
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

        protected virtual List<DatatableColumns> BindAssociateWorkoutPlanUserColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Image",
                ColumnCode = "Image",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "First Name",
                ColumnCode = "FirstName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Last Name",
                ColumnCode = "LastName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Contact",
                ColumnCode = "MobileNumber",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Associated",
                ColumnCode = "GymWorkoutPlanUserId",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
#endregion