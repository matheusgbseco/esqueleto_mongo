using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Common.Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Project.Core.Api.Controllers
{
    [Route("api/health")]
    public class HealthController : Controller
    {
        private IOptions<ConfigConnectionBase> _settings;
        private readonly IWebHostEnvironment _env;

        public HealthController(IOptions<ConfigConnectionBase> configSettings, IWebHostEnvironment env)
        {
            _settings = configSettings;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var result = new
            {
                Result = string.Format("Project.Core.Api - is live at now {0} - ConnectionString={1} - ASPNETCORE_ENVIRONMENT={2}", DateTime.Now, ExtractCns(), _env.EnvironmentName)
            };

            return Json(result);
        }

        private string ExtractCns()
        {

            if (this._settings.Value.IsNull())
                return "not load settings";

            var cns = this._settings.Value.Core;

            if (cns.IsNullOrEmpaty())
                return "not load cns";


            return string.Join("-", this._settings.Value.Core.Split(';').Where(_ => !_.Contains("password")));
        }
    }
}