using Coditech.Admin.Agents;
using Coditech.API.Client;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Coditech.Admin.Custom
{
    public static class DependencyRegistration
    {
        public static void RegisterCustomDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CoditechTranslator>();
            #region Agent
            #region Gym         
            builder.Services.AddScoped<IGymMemberDetailsAgent, GymMemberDetailsAgent>();
            builder.Services.AddScoped<IGymMembershipPlanAgent, GymMembershipPlanAgent>();
            builder.Services.AddScoped<IGymBodyMeasurementTypeAgent, GymBodyMeasurementTypeAgent>();
            builder.Services.AddScoped<IGymMemberBodyMeasurementAgent, GymMemberBodyMeasurementAgent>();
            builder.Services.AddScoped<IGymSalesInvoiceAgent, GymSalesInvoiceAgent>();
            builder.Services.AddScoped<IGymDashboardAgent, GymDashboardAgent>();
            builder.Services.AddScoped<IGymWorkoutPlanAgent, GymWorkoutPlanAgent>();
            #endregion
            #endregion Agent

            #region Client
            #region Gym         
            builder.Services.AddScoped<IGymMemberDetailsClient, GymMemberDetailsClient>();
            builder.Services.AddScoped<IGymMembershipPlanClient, GymMembershipPlanClient>();
            builder.Services.AddScoped<IGymBodyMeasurementTypeClient, GymBodyMeasurementTypeClient>();
            builder.Services.AddScoped<IGymMemberBodyMeasurementClient, GymMemberBodyMeasurementClient>();
            builder.Services.AddScoped<IGymSalesInvoiceClient, GymSalesInvoiceClient>();
            builder.Services.AddScoped<IGymDashboardClient, GymDashboardClient>();
            builder.Services.AddScoped<IGymWorkoutPlanClient, GymWorkoutPlanClient>();
            #endregion
            #endregion Client
        }
    }
}
