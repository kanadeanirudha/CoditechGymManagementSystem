
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
    public class GymMembershipPlanService : BaseService, IGymMembershipPlanService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GymMembershipPlan> _gymMembershipPlanRepository;
        private readonly ICoditechRepository<GymMemberMembershipPlan> _gymMemberMembershipPlanRepository;
        private readonly ICoditechRepository<GymMembershipPlanPackage> _gymMembershipPlanPackageRepository;
        public GymMembershipPlanService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _gymMembershipPlanRepository = new CoditechRepository<GymMembershipPlan>(_serviceProvider.GetService<Coditech_Entities>());
            _gymMemberMembershipPlanRepository = new CoditechRepository<GymMemberMembershipPlan>(_serviceProvider.GetService<Coditech_Entities>());
            _gymMembershipPlanPackageRepository = new CoditechRepository<GymMembershipPlanPackage>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual GymMembershipPlanListModel GetGymMembershipPlanList(string SelectedCentreCode, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging shipPlan.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<GymMembershipPlanModel> objStoredProc = new CoditechViewRepository<GymMembershipPlanModel>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("@CentreCode", SelectedCentreCode, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel?.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<GymMembershipPlanModel> gymMembershipPlanList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetGymMembershipPlanList @CentreCode,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 5, out pageListModel.TotalRowCount)?.ToList();
            GymMembershipPlanListModel listModel = new GymMembershipPlanListModel();

            listModel.GymMembershipPlanList = gymMembershipPlanList?.Count > 0 ? gymMembershipPlanList : new List<GymMembershipPlanModel>();
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }


        //Create Gym Membership Plan.
        public virtual GymMembershipPlanModel CreateGymMembershipPlan(GymMembershipPlanModel gymMembershipPlanModel)
        {
            if (IsNull(gymMembershipPlanModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsMembershipPlanAlreadyExist(gymMembershipPlanModel))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Membership Plan"));

            GymMembershipPlan gymMembershipPlan = gymMembershipPlanModel.FromModelToEntity<GymMembershipPlan>();

            //Create new Country and return it.
            GymMembershipPlan gymMembershipPlanData = _gymMembershipPlanRepository.Insert(gymMembershipPlan);
            if (gymMembershipPlanData?.GymMembershipPlanId > 0)
            {
                gymMembershipPlanModel.GymMembershipPlanId = gymMembershipPlanData.GymMembershipPlanId;
                SaveGymMembershipPlanPackage(gymMembershipPlanModel);
            }
            else
            {
                gymMembershipPlanModel.HasError = true;
                gymMembershipPlanModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return gymMembershipPlanModel;
        }

        //Get Gym Membership Plan
        public virtual GymMembershipPlanModel GetGymMembershipPlan(int gymMembershipPlanId)
        {
            if (gymMembershipPlanId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMembershipPlanId"));

            GymMembershipPlan gymMembershipPlan = _gymMembershipPlanRepository.Table.FirstOrDefault(x => x.GymMembershipPlanId == gymMembershipPlanId);
            GymMembershipPlanModel gymMembershipPlanModel = gymMembershipPlan?.FromEntityToModel<GymMembershipPlanModel>();
            gymMembershipPlanModel.PlanDurationType = GetEnumCodeByEnumId(gymMembershipPlanModel.PlanDurationTypeEnumId);
            gymMembershipPlanModel.PlanType = GetEnumCodeByEnumId(gymMembershipPlanModel.PlanTypeEnumId);
            gymMembershipPlanModel.IsEditable = !_gymMemberMembershipPlanRepository.Table.Any(x => x.GymMembershipPlanId == gymMembershipPlanModel.GymMembershipPlanId);
            List<GymMembershipPlanPackage> gymMembershipPlanPackageList = _gymMembershipPlanPackageRepository.Table.Where(x => x.GymMembershipPlanId == gymMembershipPlanId)?.ToList();
            gymMembershipPlanModel.SelectedGeneralServicesIds = new List<string>();
            foreach (GymMembershipPlanPackage item in gymMembershipPlanPackageList)
            {
                gymMembershipPlanModel.SelectedGeneralServicesIds.Add(item.InventoryGeneralItemLineId.ToString());
            }
            return gymMembershipPlanModel;
        }

        //Update Gym Membership Plan
        public virtual bool UpdateGymMembershipPlan(GymMembershipPlanModel gymMembershipPlanModel)
        {
            if (IsNull(gymMembershipPlanModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (gymMembershipPlanModel.GymMembershipPlanId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMembershipPlanId"));

            if (IsMembershipPlanAlreadyExist(gymMembershipPlanModel))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Membership Plan"));


            GymMembershipPlan gymMembershipPlan = gymMembershipPlanModel.FromModelToEntity<GymMembershipPlan>();

            bool isUpdated = _gymMembershipPlanRepository.Update(gymMembershipPlan);
            if (!isUpdated)
            {
                gymMembershipPlanModel.HasError = true;
                gymMembershipPlanModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            else
            {
                if (DeleteGymMembershipPlanPackage(gymMembershipPlan.GymMembershipPlanId))
                    SaveGymMembershipPlanPackage(gymMembershipPlanModel);
            }
            return isUpdated;
        }

        //Delete Gym Members
        public virtual bool DeleteGymMembershipPlan(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMembershipPlanId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("GymMembershipPlanIds", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteGymMembershipPlan @GymMembershipPlanIds,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Protected Method
        protected virtual bool DeleteGymMembershipPlanPackage(int gymMembershipPlanId)
        {
            if (IsNull(gymMembershipPlanId) || gymMembershipPlanId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "GymMembershipPlanId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>(_serviceProvider.GetService<Coditech_Entities>());
            objStoredProc.SetParameter("GymMembershipPlanId", gymMembershipPlanId, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteGymMembershipPlanPackage @GymMembershipPlanId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        //Save Gym Membership Plan Package
        protected virtual void SaveGymMembershipPlanPackage(GymMembershipPlanModel gymMembershipPlanModel)
        {
            List<GymMembershipPlanPackage> gymMembershipPlanPackageList = new List<GymMembershipPlanPackage>();
            foreach (string item in gymMembershipPlanModel.SelectedGeneralServicesIds)
            {
                gymMembershipPlanPackageList.Add(new GymMembershipPlanPackage()
                {
                    InventoryGeneralItemLineId = Convert.ToInt64(item),
                    GymMembershipPlanId = gymMembershipPlanModel.GymMembershipPlanId,
                    ServiceCost = gymMembershipPlanModel.MaxCost
                });
                gymMembershipPlanModel.MaxCost = 0;
            }
            _gymMembershipPlanPackageRepository.Insert(gymMembershipPlanPackageList);
        }


        //Check if Country code is already present or not.
        protected virtual bool IsMembershipPlanAlreadyExist(GymMembershipPlanModel gymMembershipPlanModel)
         => _gymMembershipPlanRepository.Table.Any(x => x.CentreCode == gymMembershipPlanModel.CentreCode && x.PlanTypeEnumId == gymMembershipPlanModel.PlanTypeEnumId && x.MembershipPlanName == gymMembershipPlanModel.MembershipPlanName && x.PlanDurationTypeEnumId == gymMembershipPlanModel.PlanDurationTypeEnumId && (x.GymMembershipPlanId != gymMembershipPlanModel.GymMembershipPlanId || gymMembershipPlanModel.GymMembershipPlanId == 0));
        #endregion
    }
}
