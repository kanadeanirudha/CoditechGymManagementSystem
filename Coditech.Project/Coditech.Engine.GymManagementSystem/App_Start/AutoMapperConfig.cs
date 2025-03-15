using AutoMapper;
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<FilterTuple, FilterDataTuple>().ReverseMap();
            CreateMap<OrganisationCentreMaster, OrganisationCentreModel>().ReverseMap();
            CreateMap<GeneralPerson, GeneralPersonModel>().ReverseMap();
            CreateMap<GymMemberDetailsModel, GymMemberDetails>().ReverseMap();
            CreateMap<GymMemberFollowUpModel, GymMemberFollowUp>().ReverseMap();
            CreateMap<GymBodyMeasurementType, GymBodyMeasurementTypeModel>().ReverseMap();
            CreateMap<GymMembershipPlanModel, GymMembershipPlan>().ReverseMap();
            CreateMap<GymMemberBodyMeasurement, GymMemberBodyMeasurementModel>().ReverseMap();
            CreateMap<GymMemberMembershipPlanModel, GymMemberMembershipPlan>().ReverseMap();
            CreateMap<GymMemberSalesInvoiceModel, GymMemberSalesInvoice>().ReverseMap();
            CreateMap<UserMaster, GymUserModel>().ReverseMap();
            CreateMap<GymWorkoutPlanModel, GymWorkoutPlan>().ReverseMap();
            CreateMap<GymWorkoutPlanDetailsModel, GymWorkoutPlanDetails>().ReverseMap();
            CreateMap<GymWorkoutPlanSetModel, GymWorkoutPlanSet>().ReverseMap();
            CreateMap<GymWorkoutPlanUserModel, GymWorkoutPlanUser>().ReverseMap();
        }
    }
}
