using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using System.Data;
using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class GymGeneralPersonAttendanceDetailsService : GeneralPersonAttendanceDetailsService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        const double EarthRadiusKm = 6371.0; // Radius of the Earth in kilometers

        private readonly ICoditechRepository<GeneralPersonAttendanceDetails> _generalPersonAttendanceDetailsRepository;
        public GymGeneralPersonAttendanceDetailsService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(coditechLogging,serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalPersonAttendanceDetailsRepository = new CoditechRepository<GeneralPersonAttendanceDetails>(_serviceProvider.GetService<Coditech_Entities>());
        }

        #region Protected Method
        protected override string CheckUserType(int entityId, string userType)
        {
            string centreCode = string.Empty;
            if (string.Equals(userType, UserTypeCustomEnum.GymMember.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                centreCode = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.Where(x => x.GymMemberDetailId == entityId)?.Select(y => y.CentreCode)?.FirstOrDefault();
            }
            else
            {
                centreCode = base.CheckUserType( entityId,  userType);
            }
            return centreCode;
        }

        protected override GeneralPersonModel GetGeneralPersonDetailsByEntityType(long entityId, string entityType)
        {
            long personId = 0;
            string centreCode = string.Empty;
            string personCode = string.Empty;
            short generalDepartmentMasterId = 0;
            if (entityType == UserTypeCustomEnum.GymMember.ToString())
            {
                GymMemberDetails gymMemberDetails = new CoditechRepository<GymMemberDetails>(_serviceProvider.GetService<CoditechCustom_Entities>()).Table.Where(x => x.GymMemberDetailId == entityId)?.FirstOrDefault();
                if (IsNotNull(gymMemberDetails))
                {
                    personId = gymMemberDetails.PersonId;
                    centreCode = gymMemberDetails.CentreCode;
                }
                return base.BindGeneralPersonInformation(personId, centreCode, personCode, generalDepartmentMasterId);
            }
            else
            {
                return base.GetGeneralPersonDetailsByEntityType(entityId, entityType);
            }
        }
        #endregion
    }
}
