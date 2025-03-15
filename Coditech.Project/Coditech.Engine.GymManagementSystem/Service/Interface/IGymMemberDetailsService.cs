using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

using System.Collections.Specialized;

namespace Coditech.API.Service
{
    public interface IGymMemberDetailsService
    {
        GymMemberDetailsListModel GetGymMemberDetailsList(string selectedCentreCode ,FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymMemberDetailsModel GetGymMemberOtherDetails(int gymMemberDetailId);
        bool UpdateGymMemberOtherDetails(GymMemberDetailsModel model);
        bool DeleteGymMembers(ParameterModel parameterModel);

        GymMemberFollowUpListModel GymMemberFollowUpList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);
        GymMemberFollowUpModel GetGymMemberFollowUp(long gymMemberFollowUpId);
        bool InserUpdateGymMemberFollowUp(GymMemberFollowUpModel model);
        bool DeleteGymMemberFollowUp(ParameterModel parameterModel);

        GymMemberMembershipPlanListModel GetGymMemberMembershipPlanList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);

        GymMemberMembershipPlanModel AssociateGymMemberMembershipPlan(GymMemberMembershipPlanModel gymMemberMembershipPlanModel);
        GymMemberSalesInvoiceListModel GymMemberPaymentHistoryList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength);

    }
}
