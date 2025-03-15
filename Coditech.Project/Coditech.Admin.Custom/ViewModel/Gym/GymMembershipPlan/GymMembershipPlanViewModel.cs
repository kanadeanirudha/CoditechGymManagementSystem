using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Resources;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMembershipPlanViewModel : BaseViewModel
    {
        public GymMembershipPlanViewModel()
        {
        }
        public int GymMembershipPlanId { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Membership Plan Name")]
        public string MembershipPlanName { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Maximum cost must be greater than zero.")]
        [Required]
        [Display(Name = "Max Cost")]
        public Nullable<decimal> MaxCost { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Minimum cost must be greater than zero.")]
        [Required]
        [Display(Name = "Min Cost")]
        public  Nullable<decimal> MinCost { get; set; }

        [Required]
        [Display(Name = "Plan Type")]
        public int PlanTypeEnumId { get; set; }

        [Display(Name = "Plan Type")]
        public string PlanType { get; set; }
        [Display(Name = "Plan Duration In Month")]
        public byte? PlanDurationInMonth { get; set; }
        [Display(Name = "Plan Duration In Days")]
        public byte? PlanDurationInDays { get; set; }

        [Display(Name = "Plan Duration In Session")]
        public Int16? PlanDurationInSession { get; set; }

        [Display(Name = "Is Renewable")]
        public Boolean IsRenewable { get; set; }

        [Display(Name = "Is Timebase Biometric Access")]
        public Boolean IsTimebaseBiometricAccess { get; set; }
        [Required]
        [Display(Name = "From Time")]
        public TimeSpan? FromTime { get; set; }
        [Required]
        [Display(Name = "To Time")]
        public TimeSpan? ToTime { get; set; }

        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }
        [Display(Name = "Is Tax Exclusive")]
        public Boolean IsTaxExclusive { get; set; }

        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string CentreCode { get; set; }

        [Required]
        [Display(Name = "Plan Duration Type")]
        public int PlanDurationTypeEnumId { get; set; }
        public bool IsEditable { get; set; }
        public string PlanDurationType { get; set; }

        [Required]
        [Display(Name = "Services")]
        public List<string> SelectedGeneralServicesIds { get; set; }
        public List<InventoryGeneralItemMasterModel> AllGeneralServices { get; set; }
    }
}
