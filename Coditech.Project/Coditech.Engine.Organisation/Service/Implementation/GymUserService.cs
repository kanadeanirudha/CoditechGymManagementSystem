using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Diagnostics;

using static Coditech.Common.Helper.HelperUtility;

namespace Coditech.API.Service
{
    public class GymUserService : UserService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GymMemberDetails> _gymMemberDetailsRepository;

        public GymUserService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(coditechLogging, serviceProvider, coditechEmail, coditechSMS, coditechWhatsApp)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _gymMemberDetailsRepository = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
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

        protected override void InsertPersonDetails(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList, string customData)
        {
            if (generalPersonModel.UserType.Equals(UserTypeCustomEnum.GymMember.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                InsertGymMember(generalPersonModel, settingMasterList);
            }
            else
            {
                base.InsertPersonDetails(generalPersonModel, settingMasterList);
            }
        }
        protected override bool ValidateUserwiseGeneralPerson(GeneralPersonModel generalPersonModel, ref string errorMessage, ref int generalEnumaratorId)
        {
            if (generalPersonModel.UserType.Equals(UserTypeCustomEnum.GymMember.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(generalPersonModel.SelectedCentreCode))
                {
                    errorMessage = "Selected Centre Code is Required.";
                    return false;
                }
                generalEnumaratorId = GetEnumIdByEnumCode(GeneralRunningNumberForCustomEnum.GymMemberRegistration.ToString(), GeneralEnumaratorGroupCodeEnum.GeneralRunningNumberFor.ToString());
                if (generalEnumaratorId == 0)
                {
                    errorMessage = "GymMemberRegistration is null";
                    return false;
                }
                return true;
            }
            else
            {
                return base.ValidateUserwiseGeneralPerson(generalPersonModel, ref errorMessage, ref generalEnumaratorId);
            }
        }

        private void InsertGymMember(GeneralPersonModel generalPersonModel, List<GeneralSystemGlobleSettingModel> settingMasterList)
        {
            generalPersonModel.PersonCode = GenerateRegistrationCode(GeneralRunningNumberForCustomEnum.GymMemberRegistration.ToString(), generalPersonModel.SelectedCentreCode);
            GymMemberDetails gymMemberDetails = new GymMemberDetails()
            {
                CentreCode = generalPersonModel.SelectedCentreCode,
                PersonId = generalPersonModel.PersonId,
                PersonCode = generalPersonModel.PersonCode,
                UserType = generalPersonModel.UserType
            };
            gymMemberDetails = _gymMemberDetailsRepository.Insert(gymMemberDetails);

            //Check Is Gym Member need to Login
            if (gymMemberDetails?.GymMemberDetailId > 0 && settingMasterList?.FirstOrDefault(x => x.FeatureName.Equals(GeneralSystemGlobleSettingCustomEnum.IsGymMemberLogin.ToString(), StringComparison.InvariantCultureIgnoreCase)).FeatureValue == "1")
            {
                InsertUserMasterDetails(generalPersonModel, gymMemberDetails.GymMemberDetailId, false);
                try
                {
                    GeneralEmailTemplateModel emailTemplateModel = GetEmailTemplateByCode(generalPersonModel.SelectedCentreCode, EmailTemplateCodeCustomEnum.GymMemberRegistration.ToString());
                    if (IsNotNull(emailTemplateModel) && !string.IsNullOrEmpty(emailTemplateModel?.EmailTemplateCode) && !string.IsNullOrEmpty(generalPersonModel?.EmailId))
                    {
                        string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, !string.IsNullOrEmpty(generalPersonModel.CentreName) ? generalPersonModel.CentreName : GetOrganisationCentreNameByCentreCode(generalPersonModel.SelectedCentreCode), emailTemplateModel.Subject);
                        string messageText = ReplaceGymMemberEmailTemplate(generalPersonModel, emailTemplateModel.EmailTemplate);
                        _coditechEmail.SendEmail(generalPersonModel.SelectedCentreCode, generalPersonModel.EmailId, "", subject, messageText, true);
                    }
                }
                catch (Exception ex)
                {
                    _coditechLogging.LogMessage(ex, "Gym", TraceLevel.Error);
                }
            }
        }

        protected string ReplaceGymMemberEmailTemplate(GeneralPersonModel generalPersonModel, string emailTemplate)
        {
            string messageText = emailTemplate;
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, generalPersonModel.FirstName, messageText);
            messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, generalPersonModel.LastName, messageText);
            return ReplaceEmailTemplateFooter(generalPersonModel.SelectedCentreCode, messageText);
        }
        protected override void UpdateIsActiveFlagForUserType(GeneralPersonModel generalPersonModel)
        {
            if (generalPersonModel.UserType.Equals(UserTypeCustomEnum.GymMember.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                GymMemberDetails gymMemberDetails = _gymMemberDetailsRepository.Table.Where(x => x.GymMemberDetailId == generalPersonModel.EntityId)?.FirstOrDefault();

                if (gymMemberDetails != null)
                {
                    gymMemberDetails.IsActive = generalPersonModel.IsActive;
                    _gymMemberDetailsRepository.Update(gymMemberDetails);
                }
            }
            else {
                base.UpdateIsActiveFlagForUserType(generalPersonModel);
            }
        }
    }
}
