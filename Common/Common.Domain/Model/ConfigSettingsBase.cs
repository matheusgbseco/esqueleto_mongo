using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain.Model
{
    public class ConfigSettingsBase
    {
        public string AuthorityEndPoint { get; set; }
        public string FrontApplicationUrl { get; set; }
    }
}
