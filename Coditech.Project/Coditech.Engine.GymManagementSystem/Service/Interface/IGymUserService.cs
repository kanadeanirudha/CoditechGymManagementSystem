using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IGymUserService
    {
        GymUserModel Login(UserLoginModel model);
        GymUserModel UpdateAdditionalInformation(GymUserModel gymUserModel);
        GymUserModel GetGymMemberDetails(long entityId);
    }
}
