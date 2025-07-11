using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;

using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class GymMemberDetailsService : BaseService, IGymMemberDetailsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        protected readonly ICoditechEmail _coditechEmail;
        protected readonly ICoditechSMS _coditechSMS;
        protected readonly ICoditechWhatsApp _coditechWhatsApp;
        private readonly ICoditechRepository<GymMemberDetails> _gymMemberDetailsRepository;
        private readonly ICoditechRepository<GymMemberFollowUp> _gymMemberFollowUpRepository;
        private readonly ICoditechRepository<GymMemberMembershipPlan> _gymMemberMembershipPlanRepository;
        private readonly ICoditechRepository<GymMembershipPlan> _gymMembershipPlanRepository;
        private readonly ICoditechRepository<GymMembershipPlanPackage> _gymMembershipPlanPackageRepository;
        private readonly ICoditechRepository<SalesInvoiceMaster> _salesInvoiceMasterRepository;
        private readonly ICoditechRepository<SalesInvoiceDetails> _salesInvoiceDetailsRepository;
        public GymMemberDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _coditechEmail = coditechEmail;
            _coditechSMS = coditechSMS;
            _coditechWhatsApp = coditechWhatsApp;
            _gymMemberDetailsRepository = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMemberFollowUpRepository = new CoditechRepository<GymMemberFollowUp>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMemberMembershipPlanRepository = new CoditechRepository<GymMemberMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMembershipPlanRepository = new CoditechRepository<GymMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMembershipPlanPackageRepository = new CoditechRepository<GymMembershipPlanPackage>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _salesInvoiceMasterRepository = new CoditechRepository<SalesInvoiceMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _salesInvoiceDetailsRepository = new CoditechRepository<SalesInvoiceDetails>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual GymMemberDetailsListModel GetGymMemberDetailsList(string SelectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            string listType = "";
            string isActive = filters?.Find(x => string.Equals(x.FilterName, FilterKeys.IsActive, StringComparison.CurrentCultureIgnoreCase))?.FilterValue;
            if (!string.IsNullOrEmpty(isActive))
            {
                filters.RemoveAll(x => x.FilterName == FilterKeys.IsActive);
                listType = $"and IsActive={isActive}";
            }
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GymMemberDetailsModel> objStoredProc = new CoditechViewRepository<GymMemberDetailsModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@ListType", listType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GymMemberDetailsModel> gymMemberList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMemberDetailsList @CentreCode,@listType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            GymMemberDetailsListModel listModel = new GymMemberDetailsListModel();

            listModel.GymMemberDetailsList = gymMemberList?.Count > 0 ? gymMemberList : new List<GymMemberDetailsModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Get Gym Member Other Details
        public virtual GymMemberDetailsModel GetGymMemberOtherDetails(int gymMemberDetailId)
        {
            if (gymMemberDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMemberDetailId"));

            GymMemberDetails gymMemberDetails = _gymMemberDetailsRepository.Table.FirstOrDefault(x => x.GymMemberDetailId == gymMemberDetailId);
            GymMemberDetailsModel gymMemberDetailsModel = gymMemberDetails?.FromEntityToModel<GymMemberDetailsModel>();
            if (IsNotNull(gymMemberDetailsModel))
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(gymMemberDetailsModel.PersonId);
                if (IsNotNull(gymMemberDetailsModel))
                {
                    gymMemberDetailsModel.FirstName = generalPersonModel.FirstName;
                    gymMemberDetailsModel.LastName = generalPersonModel.LastName;
                    gymMemberDetailsModel.IsActive = gymMemberDetails.IsActive;
                }
            }
            return gymMemberDetailsModel;
        }

        //Update Gym Member Other Details
        public virtual bool UpdateGymMemberOtherDetails(GymMemberDetailsModel gymMemberDetailsModel)
        {
            if (IsNull(gymMemberDetailsModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (gymMemberDetailsModel.GymMemberDetailId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMemberDetailId"));

            GymMemberDetails gymMemberDetails = _gymMemberDetailsRepository.Table.FirstOrDefault(x => x.GymMemberDetailId == gymMemberDetailsModel.GymMemberDetailId);

            bool isUpdated = false;
            if (IsNull(gymMemberDetails))
            {
                return isUpdated;
            }
            gymMemberDetails.PastInjuries = gymMemberDetailsModel.PastInjuries;
            gymMemberDetails.MedicalHistory = gymMemberDetailsModel.MedicalHistory;
            gymMemberDetails.GymGroupEnumId = gymMemberDetailsModel.GymGroupEnumId;
            gymMemberDetails.SourceEnumId = gymMemberDetailsModel.SourceEnumId;
            gymMemberDetails.OtherInformation = gymMemberDetailsModel.OtherInformation;
            gymMemberDetails.IsActive = gymMemberDetailsModel.IsActive;

            isUpdated = _gymMemberDetailsRepository.Update(gymMemberDetails);
            if (isUpdated)
            {
                ActiveInActiveUserLogin(gymMemberDetails.IsActive, Convert.ToInt64(gymMemberDetails.GymMemberDetailId), UserTypeCustomEnum.GymMember.ToString());
            }
            else
            {
                gymMemberDetailsModel.HasError = true;
                gymMemberDetailsModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isUpdated;
        }

        //Delete Gym Members
        public virtual bool DeleteGymMembers(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMemberDetailId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("GymMemberDetailIds", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteGymMembers @GymMemberDetailIds,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Member Follow Up
        public virtual GymMemberFollowUpListModel GymMemberFollowUpList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging FollowUp.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GymMemberFollowUpModel> objStoredProc = new CoditechViewRepository<GymMemberFollowUpModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GymMemberDetailId", gymMemberDetailId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GymMemberFollowUpModel> gymMemberList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMemberFollowUpList @GymMemberDetailId, @WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            GymMemberFollowUpListModel listModel = new GymMemberFollowUpListModel();

            listModel.GymMemberFollowUpList = gymMemberList?.Count > 0 ? gymMemberList : new List<GymMemberFollowUpModel>();
            listModel.BindPageListModel(pageListModel);

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
            if (IsNotNull(generalPersonModel))
            {
                listModel.FirstName = generalPersonModel.FirstName;
                listModel.LastName = generalPersonModel.LastName;
            }
            listModel.GymMemberDetailId = gymMemberDetailId;
            listModel.PersonId = personId;
            return listModel;
        }

        public virtual GymMemberFollowUpModel GetGymMemberFollowUp(long gymMemberFollowUpId)
        {
            if (gymMemberFollowUpId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "gymMemberFollowUpId"));

            GymMemberFollowUp gymMemberFollowUp = _gymMemberFollowUpRepository.Table.FirstOrDefault(x => x.GymMemberFollowUpId == gymMemberFollowUpId);
            GymMemberFollowUpModel gymMemberFollowUpModel = gymMemberFollowUp?.FromEntityToModel<GymMemberFollowUpModel>();

            return gymMemberFollowUpModel;
        }

        public virtual bool InserUpdateGymMemberFollowUp(GymMemberFollowUpModel gymMemberFollowUpModel)
        {
            if (IsNull(gymMemberFollowUpModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            gymMemberFollowUpModel.ReminderDate = gymMemberFollowUpModel.IsSetReminder ? gymMemberFollowUpModel.ReminderDate : null;
            bool isGymMemberFollowUpUpdated = false;
            GymMemberFollowUp gymMemberFollowUp = gymMemberFollowUpModel.FromModelToEntity<GymMemberFollowUp>();
            gymMemberFollowUpModel.FollowupType = GetEnumCodeByEnumId(gymMemberFollowUpModel.GymFollowupTypesEnumId);

            if (gymMemberFollowUpModel.GymMemberFollowUpId > 0)
            {
                isGymMemberFollowUpUpdated = _gymMemberFollowUpRepository.Update(gymMemberFollowUp);
            }
            else
            {
                GymMemberFollowUp gymMemberFollowUpData = _gymMemberFollowUpRepository.Insert(gymMemberFollowUp);
                isGymMemberFollowUpUpdated = gymMemberFollowUpData?.GymMemberFollowUpId > 0 ? true : false;               
            }

            if (isGymMemberFollowUpUpdated)
            {
                SendFollowUp(gymMemberFollowUpModel.GymMemberDetailId, gymMemberFollowUpModel.FollowupComment, gymMemberFollowUpModel.GymFollowupTypesEnumId);
            }
            else
            {
                
                gymMemberFollowUpModel.HasError = true;
                gymMemberFollowUpModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return isGymMemberFollowUpUpdated;
        }

        public virtual bool DeleteGymMemberFollowUp(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMemberFollowUpID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("GymMemberFollowUpId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteGymMemberFollowUp @GymMemberFollowUpId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }
        #endregion

        #region Gym Member Attendance


        #endregion

        #region GymMemberMembershipPlan
        public virtual GymMemberMembershipPlanListModel GetGymMemberMembershipPlanList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            if (gymMemberDetailId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMemberDetailId"));

            //Update Runtime Memeber Membership Plan
            UpdateRuntimeMemeberMembershipPlan(gymMemberDetailId);

            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);

            CoditechViewRepository<GymMemberMembershipPlanModel> objStoredProc = new CoditechViewRepository<GymMemberMembershipPlanModel>(_serviceProvider.GetService<CoditechCustom_Entities>());

            objStoredProc.SetParameter("@GymMemberDetailId", gymMemberDetailId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);

            List<GymMemberMembershipPlanModel> gymMemberMembershipPlanList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMemberMembershipPlanList @GymMemberDetailId, @WhereClause, @Rows, @PageNo, @Order_BY, @RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();

            GymMemberMembershipPlanListModel listModel = new GymMemberMembershipPlanListModel();
            listModel.GymMemberMembershipPlanList = gymMemberMembershipPlanList?.Count > 0 ? gymMemberMembershipPlanList : new List<GymMemberMembershipPlanModel>();
            listModel.BindPageListModel(pageListModel);
            if (personId > 0)
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
                if (IsNotNull(listModel))
                {
                    listModel.FirstName = generalPersonModel.FirstName;
                    listModel.LastName = generalPersonModel.LastName;
                }
            }
            listModel.GymMemberDetailId = gymMemberDetailId;
            listModel.PersonId = personId;

            return listModel;
        }

        //Associate Gym Member Membership Plan.
        public virtual GymMemberMembershipPlanModel AssociateGymMemberMembershipPlan(GymMemberMembershipPlanModel gymMemberMembershipPlanModel)
        {
            if (IsNull(gymMemberMembershipPlanModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            GymMembershipPlan gymMembershipPlan = _gymMembershipPlanRepository.GetById(gymMemberMembershipPlanModel.GymMembershipPlanId);

            SaveSalesInvoiceDetails(gymMemberMembershipPlanModel, gymMembershipPlan);

            gymMemberMembershipPlanModel.PlanDurationType = GetEnumCodeByEnumId(gymMembershipPlan.PlanDurationTypeEnumId);

            GymMemberMembershipPlan gymMemberMembershipPlan = gymMemberMembershipPlanModel.FromModelToEntity<GymMemberMembershipPlan>();
            gymMemberMembershipPlan.PlanAmount = gymMembershipPlan.MaxCost;
            gymMemberMembershipPlan.DiscountAmount = gymMemberMembershipPlanModel.DiscountAmount;
            gymMemberMembershipPlan.SalesInvoiceMasterId = gymMemberMembershipPlanModel.SalesInvoiceMasterId;
            if (string.Equals(gymMemberMembershipPlanModel.PlanDurationType, APIConstant.PlanDurationType, StringComparison.InvariantCultureIgnoreCase))
            {
                gymMemberMembershipPlan.PlanDurationExpirationDate = Convert.ToDateTime(gymMemberMembershipPlanModel.PlanStartDate).AddMonths(Convert.ToInt32(gymMembershipPlan.PlanDurationInMonth)).AddDays(-1).AddDays(Convert.ToInt32(gymMembershipPlan.PlanDurationInDays));
            }
            else if (string.Equals(gymMemberMembershipPlanModel.PlanDurationType, APIConstant.PlanSessionType, StringComparison.InvariantCultureIgnoreCase))
            {
                gymMemberMembershipPlan.RemainingSessionCount = Convert.ToInt16(gymMembershipPlan.PlanDurationInSession);
            }

            GymMemberMembershipPlan gymMemberMembershipPlanData = _gymMemberMembershipPlanRepository.Insert(gymMemberMembershipPlan);
            if (gymMemberMembershipPlanData?.GymMemberMembershipPlanId > 0)
            {
                gymMemberMembershipPlanModel.GymMemberMembershipPlanId = gymMemberMembershipPlanData.GymMemberMembershipPlanId;

                //Send Message for Email or SMS          
                try
                {
                    GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(gymMemberMembershipPlanModel.GymMemberDetailId, UserTypeCustomEnum.GymMember.ToString());
                    GeneralEmailTemplateModel emailTemplateModel = GetEmailTemplateByCode(generalPersonModel.SelectedCentreCode, EmailTemplateCodeCustomEnum.AssociateGymMembershipPlan.ToString());

                    if (IsNotNull(emailTemplateModel) && !string.IsNullOrEmpty(emailTemplateModel?.EmailTemplateCode) && !string.IsNullOrEmpty(generalPersonModel?.EmailId))
                    {
                        string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, !string.IsNullOrEmpty(generalPersonModel.CentreName) ? generalPersonModel.CentreName : GetOrganisationCentreNameByCentreCode(generalPersonModel.SelectedCentreCode), emailTemplateModel.Subject);
                        string messageText = ReplaceAssociateGymMembershipPlanEmailTemplate(generalPersonModel, gymMemberMembershipPlanModel, emailTemplateModel.EmailTemplate);
                        _coditechEmail.SendEmail(generalPersonModel.SelectedCentreCode, generalPersonModel.EmailId, "", subject, messageText, true);
                    }
                    GeneralEmailTemplateModel smsTemplateModel = GetSMSTemplateByCode(generalPersonModel?.SelectedCentreCode, EmailTemplateCodeCustomEnum.AssociateGymMembershipPlan.ToString());
                    if (IsNotNull(smsTemplateModel) && !string.IsNullOrEmpty(smsTemplateModel?.EmailTemplateCode) && !string.IsNullOrEmpty(generalPersonModel?.MobileNumber))
                    {
                        string messageText = ReplaceAssociateGymMembershipPlanEmailTemplate(generalPersonModel, gymMemberMembershipPlanModel, emailTemplateModel.EmailTemplate);
                        _coditechSMS.SendSMS(generalPersonModel.SelectedCentreCode, messageText, $"{generalPersonModel.CallingCode}{generalPersonModel?.MobileNumber}");
                    }
                    GeneralEmailTemplateModel whatsAppTemplateModel = GetWhatsAppTemplateByCode(generalPersonModel?.SelectedCentreCode, EmailTemplateCodeCustomEnum.AssociateGymMembershipPlan.ToString());
                    if (IsNotNull(whatsAppTemplateModel) && !string.IsNullOrEmpty(whatsAppTemplateModel?.EmailTemplateCode) && !string.IsNullOrEmpty(generalPersonModel?.MobileNumber))
                    {
                        string messageText = ReplaceAssociateGymMembershipPlanEmailTemplate(generalPersonModel, gymMemberMembershipPlanModel, emailTemplateModel.EmailTemplate);
                        _coditechWhatsApp.SendWhatsAppMessage(generalPersonModel.SelectedCentreCode, messageText, $"{generalPersonModel.CallingCode}{generalPersonModel?.MobileNumber}");
                    }
                }
                catch (Exception ex)
                {
                    _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                }
            }
            else
            {
                gymMemberMembershipPlanModel.HasError = true;
                gymMemberMembershipPlanModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return gymMemberMembershipPlanModel;
        }
        #endregion

        #region Gym Member Payment History
        public virtual GymMemberSalesInvoiceListModel GymMemberPaymentHistoryList(int gymMemberDetailId, long personId, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);

            CoditechViewRepository<GymMemberSalesInvoiceModel> objStoredProc = new CoditechViewRepository<GymMemberSalesInvoiceModel>(_serviceProvider.GetService<CoditechCustom_Entities>());

            objStoredProc.SetParameter("@GymMemberDetailId", gymMemberDetailId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);

            // Execute the stored procedure and retrieve Gym Member Payment History list.
            List<GymMemberSalesInvoiceModel> paymentHistoryList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMemberInvoiceList @GymMemberDetailId, @WhereClause, @Rows, @PageNo, @Order_BY, @RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();

            // Create and populate the GymMemberMembershipPlanListModel.
            GymMemberSalesInvoiceListModel listModel = new GymMemberSalesInvoiceListModel();
            listModel.GymMemberSalesInvoiceList = paymentHistoryList?.Count > 0 ? paymentHistoryList : new List<GymMemberSalesInvoiceModel>();
            listModel.BindPageListModel(pageListModel);
            if (personId > 0)
            {
                GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
                if (IsNotNull(listModel))
                {
                    listModel.FirstName = generalPersonModel.FirstName;
                    listModel.LastName = generalPersonModel.LastName;
                }
            }
            listModel.GymMemberDetailId = gymMemberDetailId;
            listModel.PersonId = personId;
            return listModel;
        }
        #endregion

        #region Protected Method
        protected virtual void SaveSalesInvoiceDetails(GymMemberMembershipPlanModel gymMemberMembershipPlanModel, GymMembershipPlan gymMembershipPlan)
        {
            string invoiceNumber = GenerateRegistrationCode(GeneralRunningNumberForEnum.InvoiceNumber.ToString(), gymMembershipPlan.CentreCode);
            if (string.IsNullOrEmpty(invoiceNumber))
                throw new CoditechException(ErrorCodes.NotFound, "General Running Numbers is not set for Invoice.");

            List<GymMembershipPlanPackage> gymMembershipPlanPackageList = _gymMembershipPlanPackageRepository.Table.Where(x => x.GymMembershipPlanId == gymMemberMembershipPlanModel.GymMembershipPlanId)?.ToList();

            if (gymMembershipPlanPackageList?.Count <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            List<SalesInvoiceDetails> salesInvoiceDetailList = new List<SalesInvoiceDetails>();
            foreach (GymMembershipPlanPackage gymMembershipPlanPackage in gymMembershipPlanPackageList)
            {
                InventoryGeneralItemLineDetailsModel inventoryGeneralItemLineDetails = GetInventoryGeneralItemLineDetails(gymMembershipPlanPackage.InventoryGeneralItemLineId);
                decimal totalItemLineTaxAmount = 0;
                if (gymMembershipPlanPackage.ServiceCost > 0)
                {
                    totalItemLineTaxAmount = ItemLineTaxAmount(inventoryGeneralItemLineDetails.GeneralTaxGroupMasterId, gymMembershipPlanPackage.ServiceCost);
                }
                salesInvoiceDetailList.Add(new SalesInvoiceDetails()
                {
                    InventoryGeneralItemLineId = gymMembershipPlanPackage.InventoryGeneralItemLineId,
                    ItemQuantity = 1,
                    ItemAmount = gymMembershipPlan.IsTaxExclusive ? gymMembershipPlanPackage.ServiceCost : gymMembershipPlanPackage.ServiceCost - totalItemLineTaxAmount,
                    ItemTaxAmount = totalItemLineTaxAmount,
                    GeneralTaxGroupMasterId = inventoryGeneralItemLineDetails.GeneralTaxGroupMasterId
                });
            }

            SalesInvoiceMaster salesInvoiceMaster = new SalesInvoiceMaster()
            {
                InvoiceNumber = invoiceNumber,
                TransactionDate = DateTime.Now,
                EntityId = gymMemberMembershipPlanModel.GymMemberDetailId,
                UserType = UserTypeCustomEnum.GymMember.ToString(),
                NetAmount = salesInvoiceDetailList.Sum(x => x.ItemAmount),
                TaxAmount = salesInvoiceDetailList.Sum(x => x.ItemTaxAmount),
                DiscountAmount = gymMemberMembershipPlanModel.DiscountAmount,
                TransactionReference = gymMemberMembershipPlanModel.TransactionReference,
                PaymentTypeEnumId = gymMemberMembershipPlanModel.PaymentTypeEnumId,
                Remark = gymMemberMembershipPlanModel.Remark
            };
            salesInvoiceMaster.TotalAmount = salesInvoiceMaster.NetAmount + salesInvoiceMaster.TaxAmount;
            salesInvoiceMaster.BillAmount = salesInvoiceMaster.TotalAmount - salesInvoiceMaster.DiscountAmount;

            long salesInvoiceMasterId = _salesInvoiceMasterRepository.Insert(salesInvoiceMaster).SalesInvoiceMasterId;
            salesInvoiceDetailList.ForEach(x => x.SalesInvoiceMasterId = salesInvoiceMasterId);

            _salesInvoiceDetailsRepository.Insert(salesInvoiceDetailList);
            gymMemberMembershipPlanModel.SalesInvoiceMasterId = salesInvoiceMasterId;
            gymMemberMembershipPlanModel.BillAmount = salesInvoiceMaster.BillAmount;

        }

        protected virtual void UpdateRuntimeMemeberMembershipPlan(int gymMemberDetailId)
        {
            List<GymMemberMembershipPlan> gymMemberMembershipPlanList = _gymMemberMembershipPlanRepository.Table.Where(x => x.GymMemberDetailId == gymMemberDetailId)?.ToList();
            List<long> updatedGymMemberMembershipPlanIds = new List<long>();
            foreach (GymMemberMembershipPlan item in gymMemberMembershipPlanList)
            {
                GymMembershipPlan gymMembershipPlan = _gymMembershipPlanRepository.GetById(item.GymMembershipPlanId);

                if (IsNotNull(gymMembershipPlan))
                {
                    GymMembershipPlanModel gymMembershipPlanModel = gymMembershipPlan.FromEntityToModel<GymMembershipPlanModel>();
                    gymMembershipPlanModel.PlanDurationType = GetEnumCodeByEnumId(gymMembershipPlanModel.PlanDurationTypeEnumId);
                    if (!item.IsExpired)
                    {
                        if (string.Equals(gymMembershipPlanModel.PlanDurationType, APIConstant.PlanDurationType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (item.PlanDurationExpirationDate <= DateTime.Now)
                            {
                                item.IsExpired = true;
                                updatedGymMemberMembershipPlanIds.Add(item.GymMemberMembershipPlanId);
                            }
                        }
                        else if (string.Equals(gymMembershipPlanModel.PlanDurationType, APIConstant.PlanSessionType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (item.RemainingSessionCount == 0)
                            {
                                item.IsExpired = true;
                                updatedGymMemberMembershipPlanIds.Add(item.GymMemberMembershipPlanId);
                            }
                        }
                    }
                }
            }

            if (updatedGymMemberMembershipPlanIds.Count > 0)
            {
                List<GymMemberMembershipPlan> UpdateGymMemberMembershipPlanList = gymMemberMembershipPlanList.Where(x => updatedGymMemberMembershipPlanIds.Contains(x.GymMemberMembershipPlanId))?.ToList();
                _gymMemberMembershipPlanRepository.BatchUpdate(UpdateGymMemberMembershipPlanList);
            }
        }

        protected virtual string ReplaceAssociateGymMembershipPlanEmailTemplate(GeneralPersonModel generalPersonModel, GymMemberMembershipPlanModel gymMemberMembershipPlanModel, string emailTemplate)
        {
            string messageText = emailTemplate;

            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, generalPersonModel.FirstName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, generalPersonModel.LastName, messageText);

            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.MembershipPlanName, gymMemberMembershipPlanModel.MembershipPlanName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.PlanDurationType, gymMemberMembershipPlanModel.PlanDurationType, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.PaymentMethod, GetEnumDisplayTextByEnumId(gymMemberMembershipPlanModel.PaymentTypeEnumId), messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.PaidAmount, gymMemberMembershipPlanModel.BillAmount.ToString(), messageText);
            if (string.Equals(gymMemberMembershipPlanModel.PlanDurationType, APIConstant.PlanDurationType, StringComparison.InvariantCultureIgnoreCase))
            {
                messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.PlanDuration, $"{gymMemberMembershipPlanModel.PlanStartDate.ToString()}-{gymMemberMembershipPlanModel.PlanDurationExpirationDate}", messageText);
            }
            else if (string.Equals(gymMemberMembershipPlanModel.PlanDurationType, APIConstant.PlanSessionType, StringComparison.InvariantCultureIgnoreCase))
            {
                messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.PlanDuration, $"{gymMemberMembershipPlanModel.PlanDurationInSession} Total Session", messageText);
            }
            return ReplaceEmailTemplateFooter(generalPersonModel.SelectedCentreCode, messageText);
        }


        //Send FollowUp Method
        protected virtual void SendFollowUp(int gymMemberDetailId, string followUpComment, int gymFollowupTypesEnumId)
        {
            GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(gymMemberDetailId,UserTypeCustomEnum.GymMember.ToString());
            if (generalPersonModel != null && !string.IsNullOrEmpty(followUpComment))
            {
                string followupType = GetEnumCodeByEnumId(gymFollowupTypesEnumId);
                if (followupType == GeneralFollowupTypesEnum.TextSMS.ToString())
                {
                    _coditechSMS.SendSMS(generalPersonModel.SelectedCentreCode, followUpComment, $"{generalPersonModel.CallingCode}{generalPersonModel?.MobileNumber}");
                }
                else if (followupType == GeneralFollowupTypesEnum.WhatsAppSMS.ToString())
                {
                    _coditechWhatsApp.SendWhatsAppMessage(generalPersonModel.SelectedCentreCode, followUpComment, $"{generalPersonModel.CallingCode}{generalPersonModel?.MobileNumber}");
                }
            }
            else
            {
                throw new CoditechException(ErrorCodes.InvalidData, "Follow-up comment is invalid.");
            }
        }

        protected override GeneralPersonModel GetGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeCustomEnum.GymMember.ToString())
            {
                GymMemberDetails dbtmTraineeDetails = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.FirstOrDefault(x => x.GymMemberDetailId == entityId);
                if (IsNotNull(dbtmTraineeDetails))
                {
                    personId = dbtmTraineeDetails.PersonId;
                    centreCode = dbtmTraineeDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId, dbtmTraineeDetails.IsActive);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }
        #endregion
    }


}