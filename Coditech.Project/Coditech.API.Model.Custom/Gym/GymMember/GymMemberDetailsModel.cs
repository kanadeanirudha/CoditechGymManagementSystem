namespace Coditech.Common.API.Model
{
    public class GymMemberDetailsModel : BaseModel
    {
        public int GymMemberDetailId { get; set; }
        public string CentreCode { get; set; }
        public long PersonId { get; set; }
        public string PersonCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string PastInjuries { get; set; }
        public string MedicalHistory { get; set; }
        public string OtherInformation { get; set; }
        public int? GymGroupEnumId { get; set; }
        public int? SourceEnumId { get; set; }
        public bool IsActive { get; set; }
    }
}
