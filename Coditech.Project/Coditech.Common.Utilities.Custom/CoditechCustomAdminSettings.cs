﻿using Coditech.Common.Helper;

using Microsoft.Extensions.Configuration;

namespace Coditech.Admin.Utilities
{
    public static class CoditechCustomAdminSettings
    {
        /// <summary>
        /// Gets the appsettings configuration section.
        /// </summary>
        /// <returns>The appsettings configuration section.</returns>
        private static IConfigurationSection settings = CoditechDependencyResolver.GetService<IConfiguration>().GetSection("appsettings");

        
        public static string CoditechGymManagementSystemApiRootUri
        {
            get
            {
                if (settings["EnvironmentName"] == "dev")
                    return Convert.ToString(settings["CoditechGymManagementSystemApiRootUri"]);
                else
                    return Convert.ToString($"{settings["Scheme"]}{settings["ClientName"]}-{settings["EnvironmentName"]}-api-gymmanagementsystem.{settings["ApiDomainName"]}");
            }
        }
    }
}
