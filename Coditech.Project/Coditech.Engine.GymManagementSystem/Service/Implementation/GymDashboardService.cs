
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;

using System.Data;
namespace Coditech.API.Service
{
    public class GymDashboardService : BaseService, IGymDashboardService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;

        public GymDashboardService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>(_serviceProvider.GetService<Coditech_Entities>());
        }

        //Get Gym Dashboard Details by selected Admin Role Master id.
        public virtual GymDashboardModel GetGymDashboardDetails(short numberOfDaysRecord, int selectedAdminRoleMasterId, long userMasterId)
        {
            if (selectedAdminRoleMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "SelectedAdminRoleMasterId"));

            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "UserMasterId"));

            int? dashboardFormEnumId = _adminRoleMasterRepository.Table.Where(x => x.AdminRoleMasterId == selectedAdminRoleMasterId)?.Select(y => y.DashboardFormEnumId)?.FirstOrDefault();
            GymDashboardModel gymDashboardModel = new GymDashboardModel();
            if (dashboardFormEnumId > 0)
            {
                string dashboardFormEnumCode = GetEnumCodeByEnumId((int)dashboardFormEnumId);
                gymDashboardModel.GymDashboardFormEnumCode = dashboardFormEnumCode;
                if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.GymOwnerDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataSet dataset = GetGymDashboardDetails(numberOfDaysRecord, userMasterId, DashboardFormCustomEnum.GymOwnerDashboard.ToString());
                    dataset.Tables[0].TableName = "ActiveInActiveDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    gymDashboardModel = dataTable.ConvertDataTable<GymDashboardModel>(dataset.Tables["ActiveInActiveDetails"])?.FirstOrDefault();

                    dataset.Tables[1].TableName = "FinancialOverview";
                    gymDashboardModel.TransactionOverviewList = new List<GymTransactionOverviewModel>();
                    gymDashboardModel.TransactionOverviewList = dataTable.ConvertDataTable<GymTransactionOverviewModel>(dataset.Tables["FinancialOverview"])?.ToList();

                    dataset.Tables[2].TableName = "MembershipPlanExpirationMembersActivity";
                    gymDashboardModel.GymUpcomingPlanExpirationMembersList = new List<GymUpcomingPlanExpirationMembersModel>();
                    gymDashboardModel.GymUpcomingPlanExpirationMembersList = dataTable.ConvertDataTable<GymUpcomingPlanExpirationMembersModel>(dataset.Tables["MembershipPlanExpirationMembersActivity"])?.ToList();

                    dataset.Tables[3].TableName = "RevenueByPaymentMode";
                    gymDashboardModel.RevenueByPaymentModeList = new List<GymTransactionOverviewModel>();
                    gymDashboardModel.RevenueByPaymentModeList = dataTable.ConvertDataTable<GymTransactionOverviewModel>(dataset.Tables["RevenueByPaymentMode"])?.ToList();

                    dataset.Tables[4].TableName = "LeadSource";
                    gymDashboardModel.GymGeneralLeadGenerationSourceList = new List<GymGeneralLeadGenerationSourceModel>();
                    gymDashboardModel.GymGeneralLeadGenerationSourceList = dataTable.ConvertDataTable<GymGeneralLeadGenerationSourceModel>(dataset.Tables["LeadSource"])?.ToList();

                    dataset.Tables[5].TableName = "GymUpComingEvents";
                    gymDashboardModel.GymUpcomingEventsList = new List<GymUpcomingEventsModel>();
                    gymDashboardModel.GymUpcomingEventsList = dataTable.ConvertDataTable<GymUpcomingEventsModel>(dataset.Tables["GymUpComingEvents"])?.ToList();

                    dataset.Tables[6].TableName = "YearlyFinancialOverview";
                    gymDashboardModel.YearlyFinancialOverviewList = new List<GymTransactionOverviewModel>();
                    gymDashboardModel.YearlyFinancialOverviewList = dataTable.ConvertDataTable<GymTransactionOverviewModel>(dataset.Tables["YearlyFinancialOverview"])?.ToList();

                }
                else if (dashboardFormEnumCode.Equals(DashboardFormCustomEnum.GymOperatorDashboard.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    DataSet dataset = GetGymDashboardDetails(numberOfDaysRecord, userMasterId, DashboardFormCustomEnum.GymOperatorDashboard.ToString());
                    dataset.Tables[0].TableName = "ActiveInActiveDetails";
                    ConvertDataTableToList dataTable = new ConvertDataTableToList();
                    gymDashboardModel = dataTable.ConvertDataTable<GymDashboardModel>(dataset.Tables["ActiveInActiveDetails"])?.FirstOrDefault();

                    dataset.Tables[1].TableName = "FinancialOverview";
                    gymDashboardModel.TransactionOverviewList = new List<GymTransactionOverviewModel>();
                    gymDashboardModel.TransactionOverviewList = dataTable.ConvertDataTable<GymTransactionOverviewModel>(dataset.Tables["FinancialOverview"])?.ToList();

                    dataset.Tables[2].TableName = "MembershipPlanExpirationMembersActivity";
                    gymDashboardModel.GymUpcomingPlanExpirationMembersList = new List<GymUpcomingPlanExpirationMembersModel>();
                    gymDashboardModel.GymUpcomingPlanExpirationMembersList = dataTable.ConvertDataTable<GymUpcomingPlanExpirationMembersModel>(dataset.Tables["MembershipPlanExpirationMembersActivity"])?.ToList();

                    dataset.Tables[3].TableName = "RevenueByPaymentMode";
                    gymDashboardModel.RevenueByPaymentModeList = new List<GymTransactionOverviewModel>();
                    gymDashboardModel.RevenueByPaymentModeList = dataTable.ConvertDataTable<GymTransactionOverviewModel>(dataset.Tables["RevenueByPaymentMode"])?.ToList();

                    dataset.Tables[4].TableName = "LeadSource";
                    gymDashboardModel.GymGeneralLeadGenerationSourceList = new List<GymGeneralLeadGenerationSourceModel>();
                    gymDashboardModel.GymGeneralLeadGenerationSourceList = dataTable.ConvertDataTable<GymGeneralLeadGenerationSourceModel>(dataset.Tables["LeadSource"])?.ToList();

                    dataset.Tables[5].TableName = "GymUpComingEvents";
                    gymDashboardModel.GymUpcomingEventsList = new List<GymUpcomingEventsModel>();
                    gymDashboardModel.GymUpcomingEventsList = dataTable.ConvertDataTable<GymUpcomingEventsModel>(dataset.Tables["GymUpComingEvents"])?.ToList();
                }
            }
            return gymDashboardModel;
        }
        protected virtual DataSet GetGymDashboardDetails(short numberOfDaysRecord,long userId, string dashboardForm)
        {
            ExecuteSpHelper objStoredProc = new ExecuteSpHelper(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.GetParameter("@UserId", userId, ParameterDirection.Input, SqlDbType.BigInt);
            objStoredProc.GetParameter("@NumberOfDaysRecord", numberOfDaysRecord, ParameterDirection.Input, SqlDbType.SmallInt);
            objStoredProc.GetParameter("@DashboardFor", dashboardForm, ParameterDirection.Input, SqlDbType.VarChar);
            return objStoredProc.GetSPResultInDataSet("Coditech_GetGymDashboard");
        }
    }
}
