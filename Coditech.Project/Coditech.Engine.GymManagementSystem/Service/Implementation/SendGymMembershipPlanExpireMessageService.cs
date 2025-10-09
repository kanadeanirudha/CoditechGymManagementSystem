using Coditech.API.Data;
using Coditech.Common.API;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Data;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class SendGymMembershipPlanExpireMessageService : BaseService, ISendGymMembershipPlanExpireMessageService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        protected readonly ICoditechEmail _coditechEmail;
        protected readonly ICoditechSMS _coditechSMS;
        protected readonly ICoditechWhatsApp _coditechWhatsApp;
        private readonly ICoditechRepository<GymMemberMembershipPlan> _gymMemberMembershipPlanRepository;
        private readonly ICoditechRepository<GymMembershipPlan> _gymMembershipPlanRepository;
        private readonly ICoditechRepository<GymMemberDetails> _gymMemberDetailsRepository;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;

        public SendGymMembershipPlanExpireMessageService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _coditechEmail = coditechEmail;
            _coditechSMS = coditechSMS;
            _coditechWhatsApp = coditechWhatsApp;
            _gymMemberMembershipPlanRepository = new CoditechRepository<GymMemberMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _gymMemberDetailsRepository = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _gymMembershipPlanRepository = new CoditechRepository<GymMembershipPlan>(_serviceProvider.GetService<CoditechCustom_Entities>());


        }
        public virtual bool SendGymMembershipPlanExpireMessage()
        {
            DateTime today = DateTime.Today;
            byte expirationDay = ApiCustomSettings.GymMemberPlanExpiration;

            var memberPlans = (from mp in _gymMemberMembershipPlanRepository.Table
                               join md in _gymMemberDetailsRepository.Table
                                   on mp.GymMemberDetailId equals md.GymMemberDetailId
                               join plan in _gymMembershipPlanRepository.Table
                                   on mp.GymMembershipPlanId equals plan.GymMembershipPlanId
                               where mp.PlanDurationExpirationDate.HasValue
                               select new
                               {
                                   md.PersonId,
                                   mp.PlanDurationExpirationDate,
                                   md.CentreCode,
                                   mp.GymMembershipPlanId,
                                   plan.MembershipPlanName
                               }).ToList();

            var personIds = memberPlans .Select(x => x.PersonId).Distinct().ToList();

            var persons = (from gp in _generalPersonRepository.Table
                           where personIds.Contains(gp.PersonId)
                           select new
                           {
                               gp.PersonId,
                               gp.FirstName,
                               gp.LastName,
                               gp.MobileNumber,
                               gp.EmailId,
                               gp.CallingCode
                           }).ToList();

            var result = (from mp in memberPlans
                          join gp in persons
                              on mp.PersonId equals gp.PersonId
                          select new
                          {
                              gp.FirstName,
                              gp.LastName,
                              gp.MobileNumber,
                              gp.EmailId,
                              gp.CallingCode,
                              mp.PlanDurationExpirationDate,
                              mp.CentreCode,
                              mp.GymMembershipPlanId,
                              mp.MembershipPlanName
                          }).ToList();

            result = result
                .Where(x =>
                {
                    var daysLeft = (x.PlanDurationExpirationDate.Value.Date - today).Days;
                    return daysLeft >= 0 && daysLeft <= expirationDay;
                })
                .ToList();

            if (!result.Any())
                return false;

            var groupedByCentre = result.GroupBy(x => x.CentreCode);

            foreach (var centreGroup in groupedByCentre)
            {
                var centreCode = centreGroup.Key;
                var centreName = base.GetOrganisationCentreNameByCentreCode(centreCode);

                string templateCode = EmailTemplateCodeCustomEnum.GymMembershipPlanExpireMessage.ToString();

                // --- Get Templates ---
                var emailTemplate = GetEmailTemplateByCode(centreCode, templateCode);
                var smsTemplate = GetSMSTemplateByCode(centreCode, templateCode);
                var whatsAppTemplate = GetWhatsAppTemplateByCode(centreCode, templateCode);

                if (string.IsNullOrWhiteSpace(emailTemplate?.EmailTemplate))
                    throw new CoditechException(ErrorCodes.NullModel, $"Email template '{templateCode}' not found for centre '{centreName}'.");

                string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, emailTemplate.Subject);

                foreach (var member in centreGroup)
                {
                    // --- Email ---
                    if (!string.IsNullOrEmpty(member.EmailId))
                    {
                        try
                        {
                            string emailMessage = emailTemplate.EmailTemplate;
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, member.FirstName, emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, member.LastName, emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.MembershipPlanName, member.MembershipPlanName, emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.PlanDurationExpirationDate, $"{member.PlanDurationExpirationDate:dd MMM yyyy}", emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, emailMessage);

                            _coditechEmail.SendEmail(centreCode, member.EmailId, "", subject, emailMessage, true);
                        }
                        catch (Exception ex)
                        {
                            _coditechLogging.LogMessage($"Email sending failed: {ex.Message}", CoditechLoggingEnum.Components.EmailService.ToString(), TraceLevel.Error);
                        }
                    }

                    // --- SMS ---
                    if (!string.IsNullOrEmpty(member.MobileNumber) && IsNotNull(smsTemplate))
                    {
                        try
                        {
                            string smsMessage = smsTemplate.EmailTemplate;
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, member.FirstName, smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, member.LastName, smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.MembershipPlanName, member.MembershipPlanName, smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.PlanDurationExpirationDate, $"{member.PlanDurationExpirationDate:dd MMM yyyy}", smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, smsMessage);

                            _coditechSMS.SendSMS(centreCode, smsMessage, $"{member.CallingCode}{member.MobileNumber}");
                        }
                        catch (Exception ex)
                        {
                            _coditechLogging.LogMessage($"SMS sending failed: {ex.Message}", CoditechLoggingEnum.Components.SMSService.ToString(), TraceLevel.Error);
                        }
                    }

                    // --- WhatsApp ---
                    if (!string.IsNullOrEmpty(member.MobileNumber) && IsNotNull(whatsAppTemplate))
                    {
                        try
                        {
                            string waMessage = whatsAppTemplate.EmailTemplate;
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, member.FirstName, waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, member.LastName, waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.MembershipPlanName, member.MembershipPlanName, waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.PlanDurationExpirationDate, $"{member.PlanDurationExpirationDate:dd MMM yyyy}", waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, waMessage);

                            _coditechWhatsApp.SendWhatsAppMessage(centreCode, waMessage, $"{member.CallingCode}{member.MobileNumber}");
                        }
                        catch (Exception ex)
                        {
                            _coditechLogging.LogMessage($"WhatsApp sending failed: {ex.Message}", CoditechLoggingEnum.Components.WhatsAppService.ToString(), TraceLevel.Error);
                        }
                    }
                }
            }

            return true;
        }

    }

}