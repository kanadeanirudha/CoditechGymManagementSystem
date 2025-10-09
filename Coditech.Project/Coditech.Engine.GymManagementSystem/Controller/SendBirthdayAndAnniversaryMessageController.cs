using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Coditech.API.Controllers
{
    public class SendBirthdayAndAnniversaryMessageController : BaseController
    {

        private readonly ISendBirthdayAndAnniversaryMessageService _sendBirthdayAndAnniversaryMessage;
        protected readonly ICoditechLogging _coditechLogging;
        public SendBirthdayAndAnniversaryMessageController(ICoditechLogging coditechLogging, ISendBirthdayAndAnniversaryMessageService sendBirthdayAndAnniversaryMessage)
        {
            _sendBirthdayAndAnniversaryMessage = sendBirthdayAndAnniversaryMessage;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/SendBirthdayAndAnniversaryMessage/SendBirthdayAndAnniversaryMessage")]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult SendBirthdayAndAnniversaryMessage()
        {
            try
            {
                bool result = _sendBirthdayAndAnniversaryMessage.SendBirthdayAndAnniversaryMessage();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "SendBirthdayAndAnniversaryMessage", TraceLevel.Error);
                return StatusCode(500, false); 
            }
        }
    }
}