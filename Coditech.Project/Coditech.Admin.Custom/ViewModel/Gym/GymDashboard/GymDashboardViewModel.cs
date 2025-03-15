using Coditech.Common.API.Model;
using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class GymDashboardViewModel : BaseViewModel
    {
        public GymDashboardViewModel()
        {
        }
        public int ActiveMemberCount { get; set; }
        public int InActiveMemberCount { get; set; }
        public int TotalMemberCount { get; set; }
        public Decimal ToadysTotalCollection { get; set; }
        public Decimal DaywiseTotalCollection { get; set; }
        public int TotalLeads { get; set; }
        public Decimal DigitalPaymentCollection { get; set; }
        public Decimal ManualPaymentCollection { get; set; }
        public List<GymTransactionOverviewModel> TransactionOverviewList { get; set; }
        public List<GymTransactionOverviewModel> RevenueByPaymentModeList { get; set; }
        public List<GymTransactionOverviewModel> YearlyFinancialOverviewList { get; set; }
        public List<GymUpcomingPlanExpirationMembersModel> GymUpcomingPlanExpirationMembersList { get; set; }
        public List<GymUpcomingEventsModel> GymUpcomingEventsList { get; set; }
        public List<GymGeneralLeadGenerationSourceModel> GymGeneralLeadGenerationSourceList { get; set; }
        public string GymDashboardFormEnumCode { get; set; }
        public Int16 NumberOfDaysRecord { get; set; }       
    }
}