using Coditech.API.Service;
using Coditech.Common.API;
using Coditech.Common.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Coditech.API.Controllers
{
    public class SendGymMembershipPlanExpireMessageController : BaseController
    {

        private readonly ISendGymMembershipPlanExpireMessageService _sendGymMembershipPlanExpireMessage;
        protected readonly ICoditechLogging _coditechLogging;
        public SendGymMembershipPlanExpireMessageController(ICoditechLogging coditechLogging, ISendGymMembershipPlanExpireMessageService sendGymMembershipPlanExpireMessage)
        {
            _sendGymMembershipPlanExpireMessage = sendGymMembershipPlanExpireMessage;
            _coditechLogging = coditechLogging;
        }

        [HttpGet]
        [Route("/SendGymMembershipPlanExpireMessage/SendGymMembershipPlanExpireMessage")]
        [TypeFilter(typeof(BindQueryFilter))]
        public virtual IActionResult SendGymMembershipPlanExpireMessage()
        {
            try
            {
                bool result = _sendGymMembershipPlanExpireMessage.SendGymMembershipPlanExpireMessage();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _coditechLogging.LogMessage(ex, "SendGymMembershiplPlanExpireMessage", TraceLevel.Error);
                return StatusCode(500, false);
            }
        }
    }
}