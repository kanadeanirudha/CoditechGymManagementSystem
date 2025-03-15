using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberFollowUpViewModel : BaseViewModel
    {
        public long GymMemberFollowUpId { get; set; }
        public int GymMemberDetailId { get; set; }
        public long PersonId { get; set; }

        [Required]
        [Display(Name = "Follow-Up Type")]
        public int GymFollowupTypesEnumId { get; set; }

        public string FollowupType { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Comment")]
        public string FollowupComment { get; set; }

        [Display(Name = "Set Reminder")]
        public bool IsSetReminder { get; set; }

        [Display(Name = "Reminder")]
        public DateTime? ReminderDate { get; set; }
    }
}
