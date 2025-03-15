using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class GymMemberFollowUpModel : BaseModel
    {
        public long GymMemberFollowUpId { get; set; }
        public int GymMemberDetailId { get; set; }

        [Required]
        public int GymFollowupTypesEnumId { get; set; }

        public string FollowupType { get; set; }

        [Required]
        [MaxLength(500)]
        public string FollowupComment { get; set; }

        public bool IsSetReminder { get; set; }

        public DateTime? ReminderDate { get; set; }
    }
}
