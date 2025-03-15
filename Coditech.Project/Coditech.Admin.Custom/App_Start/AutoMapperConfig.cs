using AutoMapper;
using Coditech.Admin.Agents;
using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Custom
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region Gym
            CreateMap<GymCreateEditMemberViewModel, GeneralPersonModel>().ReverseMap();
            CreateMap<GymMemberFollowUpListViewModel, GymMemberFollowUpListModel>().ReverseMap();
            CreateMap<GymMemberFollowUpViewModel, GymMemberFollowUpModel>().ReverseMap();
            CreateMap<GymBodyMeasurementTypeModel, GymBodyMeasurementTypeViewModel>().ReverseMap();
            CreateMap<GymBodyMeasurementTypeListModel, GymBodyMeasurementTypeListViewModel>().ReverseMap();
            CreateMap<GymMembershipPlanModel, GymMembershipPlanViewModel>().ReverseMap();
            CreateMap<GymMembershipPlanListModel, GymMembershipPlanListViewModel>().ReverseMap();
            CreateMap<GymMemberBodyMeasurementModel, GymMemberBodyMeasurementViewModel>().ReverseMap();
            CreateMap<GymMemberBodyMeasurementListModel, GymMemberBodyMeasurementListViewModel>().ReverseMap();
            CreateMap<GymMemberMembershipPlanModel, GymMemberMembershipPlanViewModel>().ReverseMap();
            CreateMap<GymMemberMembershipPlanListModel, GymMemberMembershipPlanListViewModel>().ReverseMap();
            CreateMap<GymMemberSalesInvoiceModel, GymMemberSalesInvoiceViewModel>().ReverseMap();
            CreateMap<GymMemberSalesInvoiceListModel, GymMemberSalesInvoiceListViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanModel, GymWorkoutPlanViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanListModel, GymWorkoutPlanListViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanDetailsModel, GymWorkoutPlanDetailsViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanSetModel, GymWorkoutPlanSetViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanUserModel, GymWorkoutPlanUserViewModel>().ReverseMap();
            CreateMap<GymWorkoutPlanUserListModel, GymWorkoutPlanUserListViewModel>().ReverseMap();
            CreateMap<GymDashboardModel, GymDashboardViewModel>().ReverseMap();

            CreateMap<GymMemberDetailsModel, GymMemberDetailsViewModel>().ReverseMap();
            CreateMap<GymMemberDetailsListModel, GymMemberDetailsListViewModel>().ReverseMap();
            #endregion
        }
    }
}
