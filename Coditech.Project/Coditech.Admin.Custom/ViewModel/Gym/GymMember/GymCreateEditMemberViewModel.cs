using Coditech.Resources;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymCreateEditMemberViewModel : GeneralPersonViewModel
    {
        public GymCreateEditMemberViewModel()
        {
        }
        public int GymMemberDetailId { get; set; }
        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string SelectedCentreCode { get; set; }
    }
}
