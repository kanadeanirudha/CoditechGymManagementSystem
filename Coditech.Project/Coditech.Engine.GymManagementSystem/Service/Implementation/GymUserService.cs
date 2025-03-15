using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class GymUserService : BaseService, IGymUserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<GymMemberDetails> _gymMemberDetailsRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        public GymUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _userMasterRepository = new CoditechRepository<UserMaster>(_serviceProvider.GetService<Coditech_Entities>());
            _gymMemberDetailsRepository = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<Coditech_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
        }

        public virtual GymUserModel Login(UserLoginModel userLoginModel)
        {
            if (IsNull(userLoginModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userLoginModel.Password = MD5Hash(userLoginModel.Password);
            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserName == userLoginModel.UserName && x.Password == userLoginModel.Password && x.UserType == UserTypeCustomEnum.GymMember.ToString());

            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            long personId = _gymMemberDetailsRepository.Table.Where(x => x.GymMemberDetailId == userMasterData.EntityId).FirstOrDefault().PersonId;

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(personId);
            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            GymUserModel userModel = new GymUserModel()
            {
                EntityId = userMasterData.EntityId,
                IsPasswordChange = userMasterData.IsPasswordChange,
                IsAcceptedTermsAndConditions = userMasterData.IsAcceptedTermsAndConditions,
                PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId),
                PersonTitle = generalPersonModel.PersonTitle,
                FirstName = generalPersonModel.FirstName,
                MiddleName = generalPersonModel.MiddleName,
                LastName = generalPersonModel.LastName
            };
            return userModel;
        }

        public virtual GymUserModel GetGymMemberDetails(long entityId)
        {
            if (entityId <= 0)
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            GymMemberDetails gymMemberDetails = _gymMemberDetailsRepository.Table.FirstOrDefault(x => x.GymMemberDetailId == entityId);
            GymUserModel userModel = new GymUserModel();

            if (IsNotNull(gymMemberDetails))
            {
                userModel.PastInjuries = gymMemberDetails.PastInjuries;
                userModel.MedicalHistory = gymMemberDetails.MedicalHistory;
                userModel.OtherInformation = gymMemberDetails.OtherInformation;
            }

            GeneralPersonModel generalPersonModel = GetGeneralPersonDetails(gymMemberDetails.PersonId);
            if (IsNull(generalPersonModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            userModel.EntityId = entityId;
            userModel.PhotoMediaPath = GetImagePath(generalPersonModel.PhotoMediaId);
            userModel.PersonTitle = generalPersonModel.PersonTitle;
            userModel.FirstName = generalPersonModel.FirstName;
            userModel.MiddleName = generalPersonModel.MiddleName;
            userModel.LastName = generalPersonModel.LastName;
            userModel.EmailId = generalPersonModel.EmailId;
            userModel.DateOfBirth = generalPersonModel.DateOfBirth;
            userModel.Gender = GetEnumDisplayTextByEnumId(generalPersonModel.GenderEnumId);
            userModel.PhoneNumber = generalPersonModel.PhoneNumber;
            userModel.MobileNumber = generalPersonModel.MobileNumber;
            userModel.EmergencyContact = generalPersonModel.EmergencyContact;
            userModel.MaritalStatus = generalPersonModel.MaritalStatus;
            userModel.BirthMark = generalPersonModel.BirthMark;
            userModel.GeneralOccupationMasterId = generalPersonModel.GeneralOccupationMasterId;
            userModel.AnniversaryDate = generalPersonModel.AnniversaryDate;
            userModel.BloodGroup = generalPersonModel.BloodGroup;
            return userModel;
        }

        //Update Additional Information
        public virtual GymUserModel UpdateAdditionalInformation(GymUserModel gymUserModel)
        {
            if (IsNull(gymUserModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            GymMemberDetails gymMemberDetails = _gymMemberDetailsRepository.Table.Where(x => x.GymMemberDetailId == gymUserModel.EntityId)?.FirstOrDefault();

            if (IsNull(gymMemberDetails))
            {
                gymUserModel.HasError = true;
                gymUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            else
            {
                UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.EntityId == gymUserModel.EntityId && x.UserType == UserTypeCustomEnum.GymMember.ToString())?.FirstOrDefault();
                gymUserModel.ModifiedBy = Convert.ToInt64(userMasterData.ModifiedBy);
                gymMemberDetails.MedicalHistory = gymUserModel.MedicalHistory;
                gymMemberDetails.PastInjuries = gymUserModel.PastInjuries;
                gymMemberDetails.OtherInformation = gymUserModel.OtherInformation;
                gymMemberDetails.ModifiedBy = gymUserModel.ModifiedBy;
                bool status = _gymMemberDetailsRepository.Update(gymMemberDetails);
                if (status)
                {
                    GeneralPerson generalPerson = _generalPersonRepository.Table.Where(x => x.PersonId == gymMemberDetails.PersonId)?.FirstOrDefault();
                    if (IsNotNull(generalPerson))
                    {
                        generalPerson.MaritalStatus = gymUserModel.MaritalStatus;
                        generalPerson.BloodGroup = gymUserModel.BloodGroup;
                        generalPerson.BirthMark = gymUserModel.BirthMark;
                        generalPerson.EmailId = gymUserModel.EmailId;
                        generalPerson.GeneralOccupationMasterId = gymUserModel.GeneralOccupationMasterId;
                        generalPerson.AnniversaryDate = gymUserModel.AnniversaryDate;
                        generalPerson.EmergencyContact = gymUserModel.EmergencyContact;
                        generalPerson.PhoneNumber = gymUserModel.PhoneNumber;
                        generalPerson.ModifiedBy = gymUserModel.ModifiedBy;
                        status = _generalPersonRepository.Update(generalPerson);
                        if (status)
                        {
                            if (IsNotNull(userMasterData))
                            {
                                userMasterData.EmailId = gymUserModel.EmailId;
                                userMasterData.ModifiedBy = gymUserModel.ModifiedBy;
                                _userMasterRepository.Update(userMasterData);
                            }
                        }
                    }
                }
                else
                {
                    gymUserModel.HasError = true;
                    gymUserModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
                }
            }
            return gymUserModel;
        }

    }
}
