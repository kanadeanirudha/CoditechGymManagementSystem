using Coditech.API.Data;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Data;
using System.Diagnostics;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class SendBirthdayAndAnniversaryMessageService : BaseService, ISendBirthdayAndAnniversaryMessageService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        protected readonly ICoditechEmail _coditechEmail;
        protected readonly ICoditechSMS _coditechSMS;
        protected readonly ICoditechWhatsApp _coditechWhatsApp;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;

        public SendBirthdayAndAnniversaryMessageService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail, ICoditechSMS coditechSMS, ICoditechWhatsApp coditechWhatsApp) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _coditechEmail = coditechEmail;
            _coditechSMS = coditechSMS;
            _coditechWhatsApp = coditechWhatsApp;
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());

        }
        public virtual bool SendBirthdayAndAnniversaryMessage()
        {
            DateTime today = DateTime.Today;

            var result = (from gp in _generalPersonRepository.Table
                          join em in _employeeMasterRepository.Table
                              on gp.PersonId equals em.PersonId
                          where
                              (gp.DateOfBirth.HasValue &&
                               gp.DateOfBirth.Value.Month == today.Month &&
                               gp.DateOfBirth.Value.Day == today.Day)
                              ||
                              (gp.AnniversaryDate.HasValue &&
                               gp.AnniversaryDate.Value.Month == today.Month &&
                               gp.AnniversaryDate.Value.Day == today.Day)
                          select new
                          {
                              gp.PersonId,
                              gp.FirstName,
                              gp.LastName,
                              gp.DateOfBirth,
                              gp.AnniversaryDate,
                              gp.EmailId,
                              gp.MobileNumber,
                              gp.CallingCode,
                              em.CentreCode
                          }).ToList();

            if (!result.Any())
                return false;

            var groupedByCentre = result.GroupBy(x => x.CentreCode);

            foreach (var centreGroup in groupedByCentre)
            {
                var centreCode = centreGroup.Key;
                var centreName = base.GetOrganisationCentreNameByCentreCode(centreCode);

                foreach (var person in centreGroup)
                {
                    string templateCode = string.Empty;

                    // -------- Decide Template ----------
                    if (person.DateOfBirth.HasValue &&
                        person.DateOfBirth.Value.Month == today.Month &&
                        person.DateOfBirth.Value.Day == today.Day)
                    {
                        templateCode = EmailTemplateCodeCustomEnum.BirthdayMessage.ToString();
                    }
                    else if (person.AnniversaryDate.HasValue &&
                             person.AnniversaryDate.Value.Month == today.Month &&
                             person.AnniversaryDate.Value.Day == today.Day)
                    {
                        templateCode = EmailTemplateCodeCustomEnum.AnniversaryMessage.ToString();
                    }

                    // --- Get Templates ---
                    var emailTemplate = GetEmailTemplateByCode(centreCode, templateCode);
                    var smsWaTemplate = GetSMSTemplateByCode(centreCode, templateCode); // same template for SMS + WhatsApp

                    if (string.IsNullOrWhiteSpace(emailTemplate?.EmailTemplate))
                        throw new CoditechException(ErrorCodes.NullModel, $"Email template '{templateCode}' not found for centre '{centreName}'.");

                    string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, emailTemplate.Subject);

                    // --------- Email ---------
                    if (!string.IsNullOrEmpty(person.EmailId))
                    {
                        try
                        {
                            string emailMessage = emailTemplate.EmailTemplate;
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, person.FirstName, emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, person.LastName, emailMessage);
                            emailMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, emailMessage);

                            _coditechEmail.SendEmail(centreCode, person.EmailId, "", subject, emailMessage, true);
                        }
                        catch (Exception ex)
                        {
                            _coditechLogging.LogMessage($"Email sending failed: {ex.Message}", CoditechLoggingEnum.Components.EmailService.ToString(), TraceLevel.Error);
                        }
                    }

                    // --------- SMS ---------
                    if (!string.IsNullOrEmpty(person.MobileNumber) && IsNotNull(smsWaTemplate))
                    {
                        try
                        {
                            string smsMessage = smsWaTemplate.EmailTemplate;
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, person.FirstName, smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, person.LastName, smsMessage);
                            smsMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, smsMessage);

                            _coditechSMS.SendSMS(centreCode, smsMessage, $"{person.CallingCode}{person.MobileNumber}");
                        }
                        catch (Exception ex)
                        {
                            _coditechLogging.LogMessage($"SMS sending failed: {ex.Message}", CoditechLoggingEnum.Components.SMSService.ToString(), TraceLevel.Error);
                        }
                    }

                    // --------- WhatsApp ---------
                    if (!string.IsNullOrEmpty(person.MobileNumber) && IsNotNull(smsWaTemplate))
                    {
                        try
                        {
                            string waMessage = smsWaTemplate.EmailTemplate;
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, person.FirstName, waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, person.LastName, waMessage);
                            waMessage = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, waMessage);

                            _coditechWhatsApp.SendWhatsAppMessage(centreCode, waMessage, $"{person.CallingCode}{person.MobileNumber}");
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