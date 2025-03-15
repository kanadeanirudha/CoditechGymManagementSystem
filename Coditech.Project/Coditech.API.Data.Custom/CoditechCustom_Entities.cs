using Microsoft.EntityFrameworkCore;

namespace Coditech.API.Data
{
    public partial class CoditechCustom_Entities : CoditechDbContext
    {
        public CoditechCustom_Entities()
        {
        }

        public CoditechCustom_Entities(DbContextOptions<CoditechCustom_Entities> options) : base(options)
        {
        }
        #region Gym
        public DbSet<GymMemberDetails> GymMemberDetails { get; set; }
        public DbSet<GymMemberFollowUp> GymMemberFollowUp { get; set; }
        public DbSet<GymBodyMeasurementType> GymBodyMeasurementType { get; set; }
        public DbSet<GymMembershipPlan> GymMembershipPlan { get; set; }
        public DbSet<GymMemberBodyMeasurement> GymMemberBodyMeasurement { get; set; }
        public DbSet<GymMemberMembershipPlan> GymMemberMembershipPlan { get; set; }
        public DbSet<GymMembershipPlanPackage> GymMembershipPlanPackage { get; set; }
        public DbSet<GymWorkoutPlan> GymWorkoutPlan { get; set; }
        public DbSet<GymWorkoutPlanDetails> GymWorkoutPlanDetails { get; set; }
        public DbSet<GymWorkoutPlanSet> GymWorkoutPlanSet { get; set; }
        public DbSet<GymWorkoutPlanUser> GymWorkoutPlanUser { get; set; }
        #endregion

    }
}
