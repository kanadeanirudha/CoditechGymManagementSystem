
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;

using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class GymSalesInvoiceService : BaseService, IGymSalesInvoiceService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<SalesInvoiceMaster> _salesInvoiceMasterRepository;
        private readonly ICoditechRepository<SalesInvoiceDetails> _salesInvoiceDetailsRepository;
        private readonly ICoditechRepository<GymMemberMembershipPlan> _gymMemberMembershipPlanRepository;
        private readonly ICoditechRepository<GymMembershipPlan> _gymMembershipPlanRepository;
        private readonly ICoditechRepository<OrganisationCentrePrintingFormat> _organisationCentrePrintingFormatRepository;
        private readonly ICoditechRepository<OrganisationCentreMaster> _organisationCentreMasterRepository;
        public GymSalesInvoiceService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _gymMemberMembershipPlanRepository = new CoditechRepository<GymMemberMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMembershipPlanRepository = new CoditechRepository<GymMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _organisationCentrePrintingFormatRepository = new CoditechRepository<OrganisationCentrePrintingFormat>(_serviceProvider.GetService<Coditech_Entities>());
            _organisationCentreMasterRepository = new CoditechRepository<OrganisationCentreMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _salesInvoiceMasterRepository = new CoditechRepository<SalesInvoiceMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _salesInvoiceDetailsRepository = new CoditechRepository<SalesInvoiceDetails>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual GymMemberSalesInvoiceListModel GymMemberServiceSalesInvoiceList(string SelectedCentreCode, DateTime? fromDate, DateTime? toDate, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            if (toDate == null && fromDate == null)
            {
                toDate = DateTime.Now;
                fromDate = Convert.ToDateTime(toDate).AddMonths(-1);
            }
            else if (fromDate != null && toDate == null)
            {
                toDate = DateTime.Now;
            }
            else if (fromDate == null && toDate != null)
            {
                fromDate = toDate.Value.AddMonths(-1);
            }
            // Bind the Filter, sorts & Paging shipPlan.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GymMemberSalesInvoiceModel> objStoredProc = new CoditechViewRepository<GymMemberSalesInvoiceModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@FromDate", fromDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@ToDate", toDate, ParameterDirection.Input, DbType.Date);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GymMemberSalesInvoiceModel> gymMemberSalesInvoiceList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMemberServiceInvoiceList @CentreCode,@FromDate,@ToDate,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 7, out pageListModel.TotalRowCount)?.ToList();

            GymMemberSalesInvoiceListModel listModel = new GymMemberSalesInvoiceListModel();
            listModel.GymMemberSalesInvoiceList = gymMemberSalesInvoiceList?.Count > 0 ? gymMemberSalesInvoiceList : new List<GymMemberSalesInvoiceModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        public virtual SalesInvoicePrintModel GetSalesInvoiceDetails(long salesInvoiceMasterId)
        {

            if (salesInvoiceMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "SalesInvoiceMasterId"));

            SalesInvoicePrintModel salesInvoicePrintModel = null;
            SalesInvoiceMaster salesInvoiceMaster = _salesInvoiceMasterRepository.Table.FirstOrDefault(x => x.SalesInvoiceMasterId == salesInvoiceMasterId);
            if (IsNotNull(salesInvoiceMaster))
            {
                GymMemberMembershipPlan gymMemberMembershipPlan = _gymMemberMembershipPlanRepository.Table.FirstOrDefault(x => x.SalesInvoiceMasterId == salesInvoiceMaster.SalesInvoiceMasterId);
                salesInvoicePrintModel = new SalesInvoicePrintModel()
                {
                    SalesInvoiceMasterId = salesInvoiceMaster.SalesInvoiceMasterId,
                    EntityId = salesInvoiceMaster.EntityId,
                    UserType = salesInvoiceMaster.UserType,
                    TransactionDate = salesInvoiceMaster.TransactionDate,
                    InvoiceNumber = salesInvoiceMaster.InvoiceNumber,
                    NetAmount = salesInvoiceMaster.NetAmount,
                    TaxAmount = salesInvoiceMaster.TaxAmount,
                    DiscountAmount = salesInvoiceMaster.DiscountAmount,
                    BillAmount = salesInvoiceMaster.BillAmount,
                    TotalAmount = salesInvoiceMaster.TotalAmount,
                    PaymentType = GetEnumDisplayTextByEnumId(gymMemberMembershipPlan.PaymentTypeEnumId),
                    TransactionReference = gymMemberMembershipPlan.TransactionReference
                };

                GymMembershipPlan gymMembershipPlan = _gymMembershipPlanRepository.GetById(gymMemberMembershipPlan.GymMembershipPlanId);
                List<SalesInvoiceDetails> salesInvoiceDetailList = _salesInvoiceDetailsRepository.Table.Where(x => x.SalesInvoiceMasterId == salesInvoicePrintModel.SalesInvoiceMasterId)?.ToList();
                salesInvoicePrintModel.SalesInvoiceDetailsList = new List<SalesInvoiceDetailsPrintModel>();
                foreach (SalesInvoiceDetails item in salesInvoiceDetailList)
                {
                    InventoryGeneralItemLineDetailsModel inventoryGeneralItemLineDetails = GetInventoryGeneralItemLineDetails(item.InventoryGeneralItemLineId);
                    salesInvoicePrintModel.SalesInvoiceDetailsList.Add(new SalesInvoiceDetailsPrintModel()
                    {
                        HSNSACCode = inventoryGeneralItemLineDetails.HSNSACCode,
                        ItemName = inventoryGeneralItemLineDetails.ItemName,
                        ItemDescription = gymMembershipPlan.MembershipPlanName,
                        ItemQuantity = item.ItemQuantity,
                        ItemAmount = item.ItemAmount,
                        ItemTaxAmount = item.ItemTaxAmount,
                    });
                }

                GeneralPersonModel generalPersonModel = GetGeneralPersonDetailsByEntityType(salesInvoicePrintModel.EntityId, salesInvoicePrintModel.UserType);
                if (IsNotNull(generalPersonModel))
                {
                    salesInvoicePrintModel.CentreCode = generalPersonModel.SelectedCentreCode;
                    salesInvoicePrintModel.CustomerInformation = new SalesInvoiceCustomerInformationModel()
                    {
                        PersonTitle = generalPersonModel.PersonTitle,
                        FirstName = generalPersonModel.FirstName,
                        MiddleName = generalPersonModel.MiddleName,
                        LastName = generalPersonModel.LastName,
                        MobileNumber = generalPersonModel.MobileNumber,
                        EmailId = generalPersonModel.EmailId,
                    };
                }
                int organisationCentreMasterId = GetOrganisationCentreMasterIdByCentreCode(salesInvoicePrintModel.CentreCode);
                OrganisationCentrePrintingFormat organisationCentrePrintingFormat = _organisationCentrePrintingFormatRepository.Table.FirstOrDefault(x => x.OrganisationCentreMasterId == organisationCentreMasterId);
                salesInvoicePrintModel.OrganisationCentreInvoicePrintingFormat = new OrganisationCentreInvoicePrintingFormat();
                if (IsNotNull(organisationCentrePrintingFormat))
                {
                    salesInvoicePrintModel.OrganisationCentreInvoicePrintingFormat.PrintingLine1 = organisationCentrePrintingFormat.PrintingLine1;
                    salesInvoicePrintModel.OrganisationCentreInvoicePrintingFormat.PrintingLine2 = organisationCentrePrintingFormat.PrintingLine2;
                    salesInvoicePrintModel.OrganisationCentreInvoicePrintingFormat.PrintingLine3 = organisationCentrePrintingFormat.PrintingLine3;
                    salesInvoicePrintModel.OrganisationCentreInvoicePrintingFormat.PrintingLine4 = organisationCentrePrintingFormat.PrintingLine4;
                }
                salesInvoicePrintModel.OrganisationCentreModel = _organisationCentreMasterRepository.Table.FirstOrDefault(x => x.OrganisationCentreMasterId == organisationCentreMasterId)?.FromEntityToModel<OrganisationCentreModel>();
            }
            return salesInvoicePrintModel;
        }
    }
}
