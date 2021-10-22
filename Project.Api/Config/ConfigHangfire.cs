using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using Project.Core.Services;

namespace Project.Core.Api.Config
{
    public class ConfigHangfire
    {
        public static void RegisterTasks(IConfigurationSection config)
        {
            if (Convert.ToBoolean(config["Enabled"]))
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                //RecurringJob.RemoveIfExists("NAME_SERVICE");
                //RecurringJob.AddOrUpdate<NameService>("NAME_SERVICE", _service => _service.Import(), "0/2 * * * *", tz);
            }
        }

    }
}
