using Coditech.Common.Helper;

using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class GymMemberDetailsViewModel : BaseViewModel
    {
        public GymMemberDetailsViewModel()
        {
        }
        public int GymMemberDetailId { get; set; }
        public string CentreCode { get; set; }
        public long PersonId { get; set; }

        [MaxLength(50)]
        [Editable(false)]
        public string PersonCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }

        [MaxLength(500)]
        [Display(Name = "Past Injuries")]
        public string PastInjuries { get; set; }

        [MaxLength(500)]
        [Display(Name = "Medical History")]
        public string MedicalHistory { get; set; }

        [MaxLength(500)]
        [Display(Name = "Other Information")]
        public string OtherInformation { get; set; }

        [Required]
        [Display(Name ="Group")]
        public int GymGroupEnumId { get; set; }
        [Required]
        [Display(Name = "Source")]
        public int? SourceEnumId { get; set; }
        public string ImagePath { get; set; }
        public string MobileNumber { get; set; }
        public string SelectedCentreCode { get; set; }  
        public string EmailId { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
