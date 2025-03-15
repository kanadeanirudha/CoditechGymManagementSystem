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
    public class GymMemberDetailsAgent : BaseAgent, IGymMemberDetailsAgent
    {
        #region Private Variable
        protected readonly ICoditechLogging _coditechLogging;
        private readonly IGymMemberDetailsClient _gymMemberDetailsClient;
        private readonly IGeneralPersonAttendanceDetailsClient _generalPersonAttendanceDetailsClient;
        private readonly IUserClient _userClient;
        #endregion

        #region Public Constructor
        public GymMemberDetailsAgent(ICoditechLogging coditechLogging, IGymMemberDetailsClient gymMemberDetailsClient, IUserClient userClient, IGeneralPersonAttendanceDetailsClient generalPersonAttendanceDetailsClient)
        {
            _coditechLogging = coditechLogging;
            _gymMemberDetailsClient = GetClient<IGymMemberDetailsClient>(gymMemberDetailsClient);
            _generalPersonAttendanceDetailsClient = GetClient<IGeneralPersonAttendanceDetailsClient>(generalPersonAttendanceDetailsClient);
            _userClient = GetClient<IUserClient>(userClient);
        }
        #endregion

        #region Public Methods
        public virtual GymMemberDetailsListViewModel GetGymMemberDetailsList(DataTableViewModel dataTableModel, string listType = null)
        {
            FilterCollection filters = new FilterCollection();
            if (listType == "Active")
            {
                filters.Add("IsActive", ProcedureFilterOperators.Equals, "1");
            }
            else if (listType == "InActive")
            {
                filters.Add("IsActive", ProcedureFilterOperators.Equals, "0");
            }

            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters.Add("FirstName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LastName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("EmailId", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MobileNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("PersonCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "FirstName" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymMemberDetailsListResponse response = _gymMemberDetailsClient.List(dataTableModel.SelectedCentreCode, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMemberDetailsListModel gymMemberList = new GymMemberDetailsListModel { GymMemberDetailsList = response?.GymMemberDetailsList };
            GymMemberDetailsListViewModel listViewModel = new GymMemberDetailsListViewModel();
            listViewModel.GymMemberDetailsList = gymMemberList?.GymMemberDetailsList?.ToViewModel<GymMemberDetailsViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMemberDetailsList.Count, BindColumns());
            return listViewModel;
        }

        #region General Person
        //Create Member Details
        public virtual GymCreateEditMemberViewModel CreateMemberDetails(GymCreateEditMemberViewModel gymCreateEditMemberViewModel)
        {
            try
            {
                gymCreateEditMemberViewModel.UserType = UserTypeCustomEnum.GymMember.ToString();
                GeneralPersonResponse response = _userClient.InsertPersonInformation(gymCreateEditMemberViewModel.ToModel<GeneralPersonModel>());
                GeneralPersonModel generalPersonModel = response?.GeneralPersonModel;
                return IsNotNull(generalPersonModel) ? generalPersonModel.ToViewModel<GymCreateEditMemberViewModel>() : new GymCreateEditMemberViewModel();
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GymCreateEditMemberViewModel)GetViewModelWithErrorMessage(gymCreateEditMemberViewModel, ex.ErrorMessage);
                    default:
                        return (GymCreateEditMemberViewModel)GetViewModelWithErrorMessage(gymCreateEditMemberViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymCreateEditMemberViewModel)GetViewModelWithErrorMessage(gymCreateEditMemberViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get Member Personal Details by personId.
        public virtual GymCreateEditMemberViewModel GetMemberPersonalDetails(int gymMemberDetailId, long personId)
        {
            GeneralPersonResponse response = _userClient.GetPersonInformation(personId);
            GymCreateEditMemberViewModel gymCreateEditMemberViewModel = response?.GeneralPersonModel.ToViewModel<GymCreateEditMemberViewModel>();
            if (IsNotNull(gymCreateEditMemberViewModel))
            {
                GymMemberDetailsResponse gymMemberDetailsResponse = _gymMemberDetailsClient.GetGymMemberOtherDetails(gymMemberDetailId);
                if (IsNotNull(gymMemberDetailsResponse))
                {
                    gymCreateEditMemberViewModel.SelectedCentreCode = gymMemberDetailsResponse.GymMemberDetailsModel.CentreCode;
                }
                gymCreateEditMemberViewModel.GymMemberDetailId = gymMemberDetailId;
                gymCreateEditMemberViewModel.PersonId = personId;
            }
            return gymCreateEditMemberViewModel;
        }

        //Update Member Details
        public virtual GymCreateEditMemberViewModel UpdateMemberPersonalDetails(GymCreateEditMemberViewModel gymCreateEditMemberViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                GeneralPersonModel generalPersonModel = gymCreateEditMemberViewModel.ToModel<GeneralPersonModel>();
                generalPersonModel.EntityId = gymCreateEditMemberViewModel.GymMemberDetailId;
                generalPersonModel.UserType = UserTypeCustomEnum.GymMember.ToString();
                GeneralPersonResponse response = _userClient.UpdatePersonInformation(generalPersonModel);
                generalPersonModel = response?.GeneralPersonModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(generalPersonModel) ? generalPersonModel.ToViewModel<GymCreateEditMemberViewModel>() : (GymCreateEditMemberViewModel)GetViewModelWithErrorMessage(new GymCreateEditMemberViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymCreateEditMemberViewModel)GetViewModelWithErrorMessage(gymCreateEditMemberViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        #endregion

        #region Member Other Details
        //Get Member Other Details
        public virtual GymMemberDetailsViewModel GetGymMemberOtherDetails(int gymMemberDetailId)
        {
            GymMemberDetailsResponse response = _gymMemberDetailsClient.GetGymMemberOtherDetails(gymMemberDetailId);
            GymMemberDetailsViewModel gymMemberDetailsViewModel = response?.GymMemberDetailsModel.ToViewModel<GymMemberDetailsViewModel>();
            return gymMemberDetailsViewModel;
        }

        //Update Gym Member Other Details.
        public virtual GymMemberDetailsViewModel UpdateGymMemberOtherDetails(GymMemberDetailsViewModel gymMemberDetailsViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                GymMemberDetailsResponse response = _gymMemberDetailsClient.UpdateGymMemberOtherDetails(gymMemberDetailsViewModel.ToModel<GymMemberDetailsModel>());
                GymMemberDetailsModel gymMemberDetailsModel = response?.GymMemberDetailsModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(gymMemberDetailsModel) ? gymMemberDetailsModel.ToViewModel<GymMemberDetailsViewModel>() : (GymMemberDetailsViewModel)GetViewModelWithErrorMessage(new GymMemberDetailsViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymMemberDetailsViewModel)GetViewModelWithErrorMessage(gymMemberDetailsViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
        //Delete gym Member Details.
        public virtual bool DeleteGymMembers(string gymMemberDetailIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymMemberDetailsClient.DeleteGymMembers(new ParameterModel { Ids = gymMemberDetailIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymMemberDetails;
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
        #endregion

        #region Member Follow Up
        public virtual GymMemberFollowUpListViewModel GymMemberFollowUpList(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                //filters.Add("FollowupType", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("FollowupComment", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymMemberFollowUpListResponse response = _gymMemberDetailsClient.GymMemberFollowUpList(gymMemberDetailId, personId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMemberFollowUpListModel gymMemberList = new GymMemberFollowUpListModel { GymMemberFollowUpList = response?.GymMemberFollowUpList };

            GymMemberFollowUpListViewModel listViewModel = new GymMemberFollowUpListViewModel()
            {
                PersonId = response.PersonId,
                GymMemberDetailId = response.GymMemberDetailId,
                FirstName = response?.FirstName,
                LastName = response?.LastName
            };
            listViewModel.GymMemberFollowUpList = gymMemberList?.GymMemberFollowUpList?.ToViewModel<GymMemberFollowUpViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMemberFollowUpList.Count, BindGymMemberFollowUpColumns());
            return listViewModel;
        }

        public virtual GymMemberFollowUpViewModel GetMemberFollowUp(long gymMemberFollowUpId)
        {
            GymMemberFollowUpResponse response = _gymMemberDetailsClient.GetGymMemberFollowUp(gymMemberFollowUpId);
            GymMemberFollowUpViewModel gymMemberDetailsViewModel = response?.GymMemberFollowUpModel.ToViewModel<GymMemberFollowUpViewModel>();
            return gymMemberDetailsViewModel;
        }

        //Update Member Details
        public virtual GymMemberFollowUpViewModel InserUpdateGymMemberFollowUp(GymMemberFollowUpViewModel gymMemberFollowUpViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                GymMemberFollowUpResponse response = _gymMemberDetailsClient.InserUpdateGymMemberFollowUp(gymMemberFollowUpViewModel.ToModel<GymMemberFollowUpModel>());
                GymMemberFollowUpModel gymMemberFollowUpModel = response?.GymMemberFollowUpModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(gymMemberFollowUpModel) ? gymMemberFollowUpModel.ToViewModel<GymMemberFollowUpViewModel>() : (GymMemberFollowUpViewModel)GetViewModelWithErrorMessage(new GymMemberFollowUpViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymMemberFollowUpViewModel)GetViewModelWithErrorMessage(gymMemberFollowUpViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete gym Member Details.
        public virtual bool DeleteGymMemberFollowUp(string gymMemberFollowUpIdIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;

            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _gymMemberDetailsClient.DeleteGymMemberFollowUp(new ParameterModel { Ids = gymMemberFollowUpIdIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymMemberDetails;
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
        #endregion

        #region Gym Member Attendance
        public virtual GeneralPersonAttendanceDetailsListViewModel GeneralPersonAttendanceDetailsList(int gymMemberDetailId, long personId, string userType, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("AttendanceDate", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LoginTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LogoutTime", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GeneralPersonAttendanceDetailsListResponse response = _generalPersonAttendanceDetailsClient.GeneralPersonAttendanceDetailsList(gymMemberDetailId, userType, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralPersonAttendanceDetailsListModel gymMemberList = new GeneralPersonAttendanceDetailsListModel { GeneralPersonAttendanceDetailsList = response?.GeneralPersonAttendanceDetailsList };

            GeneralPersonAttendanceDetailsListViewModel listViewModel = new GeneralPersonAttendanceDetailsListViewModel()
            {
                PersonId = response.PersonId,
                EntityId = gymMemberDetailId,
                FirstName = response?.FirstName,
                LastName = response?.LastName
            };
            listViewModel.GeneralPersonAttendanceDetailsList = gymMemberList?.GeneralPersonAttendanceDetailsList?.ToViewModel<GeneralPersonAttendanceDetailsViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GeneralPersonAttendanceDetailsList.Count, BindGymMemberAttendanceDeatailsColumns(), false);
            return listViewModel;
        }

        public virtual GeneralPersonAttendanceDetailsViewModel GetGeneralPersonAttendanceDetails(long generalPersonAttendanceDetailsId)
        {
            GeneralPersonAttendanceDetailsResponse response = _generalPersonAttendanceDetailsClient.GetGeneralPersonAttendanceDetails(generalPersonAttendanceDetailsId);
            GeneralPersonAttendanceDetailsViewModel gymMemberDetailsViewModel = response?.GeneralPersonAttendanceDetailsModel.ToViewModel<GeneralPersonAttendanceDetailsViewModel>();
            return gymMemberDetailsViewModel;
        }

        //Update Member Details
        public virtual GeneralPersonAttendanceDetailsViewModel InserUpdateGeneralPersonAttendanceDetails(GeneralPersonAttendanceDetailsViewModel generalPersonAttendanceDetailsViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                generalPersonAttendanceDetailsViewModel.EntityId = generalPersonAttendanceDetailsViewModel.EntityId;
                generalPersonAttendanceDetailsViewModel.UserType = UserTypeCustomEnum.GymMember.ToString();
                GeneralPersonAttendanceDetailsResponse response = _generalPersonAttendanceDetailsClient.InserUpdateGeneralPersonAttendanceDetails(generalPersonAttendanceDetailsViewModel.ToModel<GeneralPersonAttendanceDetailsModel>());
                GeneralPersonAttendanceDetailsModel generalPersonAttendanceDetailsModel = response?.GeneralPersonAttendanceDetailsModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(generalPersonAttendanceDetailsModel) ? generalPersonAttendanceDetailsModel.ToViewModel<GeneralPersonAttendanceDetailsViewModel>() : (GeneralPersonAttendanceDetailsViewModel)GetViewModelWithErrorMessage(new GeneralPersonAttendanceDetailsViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GeneralPersonAttendanceDetailsViewModel)GetViewModelWithErrorMessage(generalPersonAttendanceDetailsViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete gym Member Details.
        public virtual bool DeleteGeneralPersonAttendanceDetails(string generalPersonAttendanceDetailsIdIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                TrueFalseResponse trueFalseResponse = _generalPersonAttendanceDetailsClient.DeleteGeneralPersonAttendanceDetails(new ParameterModel { Ids = generalPersonAttendanceDetailsIdIds });
                return trueFalseResponse.IsSuccess;
            }
            catch (CoditechException ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Warning);
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AssociationDeleteError:
                        errorMessage = AdminResources.ErrorDeleteGymMemberDetails;
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
        #endregion

        #region Gym Member Membership Plan
        public virtual GymMemberMembershipPlanListViewModel GetGymMemberMembershipPlan(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("MembershipPlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymMemberMembershipPlanListResponse response = _gymMemberDetailsClient.GetGymMemberMembershipPlanList(gymMemberDetailId, personId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMemberMembershipPlanListModel gymMemberMembershipPlanList = new GymMemberMembershipPlanListModel { GymMemberMembershipPlanList = response?.GymMemberMembershipPlanList };

            GymMemberMembershipPlanListViewModel listViewModel = new GymMemberMembershipPlanListViewModel()
            {
                PersonId = personId,
                GymMemberDetailId = gymMemberDetailId,
                FirstName = response?.FirstName,
                LastName = response?.LastName
            };
            listViewModel.GymMemberMembershipPlanList = gymMemberMembershipPlanList?.GymMemberMembershipPlanList?.ToViewModel<GymMemberMembershipPlanViewModel>().ToList();
            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMemberMembershipPlanList.Count, BindGymMemberMembershipPlanColumns(), false);
            return listViewModel;
        }

        //Associate Gym Member Membership Plan
        public virtual GymMemberMembershipPlanViewModel AssociateGymMemberMembershipPlan(GymMemberMembershipPlanViewModel gymMemberMembershipPlanViewModel)
        {
            try
            {
                _coditechLogging.LogMessage("Agent method execution started.", "Gym", TraceLevel.Info);
                GymMemberMembershipPlanResponse response = _gymMemberDetailsClient.AssociateGymMemberMembershipPlan(gymMemberMembershipPlanViewModel.ToModel<GymMemberMembershipPlanModel>());
                GymMemberMembershipPlanModel gymMemberMembershipPlanModel = response?.GymMemberMembershipPlanModel;
                _coditechLogging.LogMessage("Agent method execution done.", "Gym", TraceLevel.Info);
                return IsNotNull(gymMemberMembershipPlanModel) ? gymMemberMembershipPlanModel.ToViewModel<GymMemberMembershipPlanViewModel>() : (GymMemberMembershipPlanViewModel)GetViewModelWithErrorMessage(new GymMemberMembershipPlanViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                return (GymMemberMembershipPlanViewModel)GetViewModelWithErrorMessage(gymMemberMembershipPlanViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        public virtual GymMemberSalesInvoiceListViewModel GymMemberPaymentHistoryList(int gymMemberDetailId, long personId, DataTableViewModel dataTableModel)
        {
            FilterCollection filters = null;
            dataTableModel = dataTableModel ?? new DataTableViewModel();
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("InvoiceNumber", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("MembershipPlanName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            SortCollection sortlist = SortingData(dataTableModel.SortByColumn = string.IsNullOrEmpty(dataTableModel.SortByColumn) ? "" : dataTableModel.SortByColumn, dataTableModel.SortBy);

            GymMemberSalesInvoiceListResponse response = _gymMemberDetailsClient.GymMemberPaymentHistoryList(gymMemberDetailId, personId, null, filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GymMemberSalesInvoiceListModel gymMemberList = new GymMemberSalesInvoiceListModel { GymMemberSalesInvoiceList = response?.GymMemberSalesInvoiceList };

            GymMemberSalesInvoiceListViewModel listViewModel = new GymMemberSalesInvoiceListViewModel()
            {
                GymMemberDetailId = response.GymMemberDetailId,
                PersonId = response.PersonId,
                FirstName = response?.FirstName,
                LastName = response?.LastName
            };
            listViewModel.GymMemberSalesInvoiceList = gymMemberList?.GymMemberSalesInvoiceList?.ToViewModel<GymMemberSalesInvoiceViewModel>().ToList();

            SetListPagingData(listViewModel.PageListViewModel, response, dataTableModel, listViewModel.GymMemberSalesInvoiceList.Count, BindGymMemberSalesInvoiceColumns(), false);
            return listViewModel;
        }
        #endregion
        #region protected
        protected virtual List<DatatableColumns> BindColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Image",
                ColumnCode = "Image",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Member Code",
                ColumnCode = "PersonCode",
                IsSortable = true,
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
                ColumnName = "Email Id",
                ColumnCode = "EmailId",
                IsSortable = true,
            });           
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "IsActive",
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
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Follow-Up Type",
                ColumnCode = "FollowupType",
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
                IsSortable = true,
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindGymMemberAttendanceDeatailsColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Attendance Date",
                ColumnCode = "AttendanceDate",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Check In Time",
                ColumnCode = "LoginTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Check Out Time",
                ColumnCode = "LogoutTime",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Duration",
                ColumnCode = "Duration",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Remark",
                ColumnCode = "Remark",
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindGymMemberSalesInvoiceColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Invoice Number",
                ColumnCode = "InvoiceNumber",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Transaction Date",
                ColumnCode = "TransactionDate",
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
                ColumnName = "Plan Name",
                ColumnCode = "MembershipPlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Paid Amount",
                ColumnCode = "BillAmount",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Payment Mode",
                ColumnCode = "PaymentType",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Payment Received By",
                ColumnCode = "PaymentReceivedBy",
            });
            return datatableColumnList;
        }

        protected virtual List<DatatableColumns> BindGymMemberMembershipPlanColumns()
        {
            List<DatatableColumns> datatableColumnList = new List<DatatableColumns>();
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Type",
                ColumnCode = "PlanType",
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Plan Name",
                ColumnCode = "MembershipPlanName",
                IsSortable = true,
            });
            datatableColumnList.Add(new DatatableColumns()
            {
                ColumnName = "Is Expired",
                ColumnCode = "IsExpired",
                IsSortable = true,
            });
            return datatableColumnList;
        }
        #endregion
    }
}
#endregion