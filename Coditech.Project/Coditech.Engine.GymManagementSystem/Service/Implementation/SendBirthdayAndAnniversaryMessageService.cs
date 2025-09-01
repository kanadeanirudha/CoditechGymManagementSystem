using Coditech.API.Data;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using System.Data;
namespace Coditech.API.Service
{
    public class SendBirthdayAndAnniversaryMessageService : BaseService, ISendBirthdayAndAnniversaryMessageService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<EmployeeMaster> _employeeMasterRepository;

        public SendBirthdayAndAnniversaryMessageService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider, ICoditechEmail coditechEmail) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _employeeMasterRepository = new CoditechRepository<EmployeeMaster>(_serviceProvider.GetService<Coditech_Entities>());

        }
        public virtual bool SendBirthdayAndAnniversaryMessage()
        {
            //// Test ke liye fixed date
            var today = new DateTime(2003, 1, 1);

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
                              em.CentreCode
                          }).ToList();

            if (!result.Any())
                return false; 

            var groupedByCentre = result.GroupBy(x => x.CentreCode);

            foreach (var centreGroup in groupedByCentre)
            {
                var centreCode = centreGroup.Key;
                var centreName = base.GetOrganisationCentreNameByCentreCode(centreCode);

                string templateCode = EmailTemplateCodeCustomEnum.SendBirthdayAndAnniversaryMessage.ToString();
                var emailTemplate = GetEmailTemplateByCode(centreCode, templateCode);

                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.EmailTemplate))
                    throw new CoditechException(ErrorCodes.NullModel, $"Email template '{templateCode}' not found for centre '{centreName}'.");

                string subject = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, emailTemplate.Subject);

                foreach (var person in centreGroup)
                {
                    string message = string.Empty;

                    if (person.DateOfBirth.HasValue &&
                        person.DateOfBirth.Value.Month == today.Month &&
                        person.DateOfBirth.Value.Day == today.Day)
                    {
                        message = $"🎉 Happy Birthday!";
                    }
                    else if (person.AnniversaryDate.HasValue &&
                             person.AnniversaryDate.Value.Month == today.Month &&
                             person.AnniversaryDate.Value.Day == today.Day)
                    {
                        int years = today.Year - person.AnniversaryDate.Value.Year;
                        message = $"💐 Happy {years} Year Anniversary!";
                    }

                    string messageText = emailTemplate.EmailTemplate;
                    messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.FirstName, person.FirstName, messageText);
                    messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.LastName, person.LastName, messageText);
                    messageText = ReplaceTokenWithMessageText(EmailTemplateTokenCustomConstant.BirthdayAndAnniversaryMessage, message, messageText);
                    messageText = ReplaceTokenWithMessageText(EmailTemplateTokenConstant.CentreName, centreName, messageText);

                    _coditechEmail.SendEmail(centreCode, person.EmailId, "", subject, messageText, true);
                }
            }

            return true;
        }


    }

}