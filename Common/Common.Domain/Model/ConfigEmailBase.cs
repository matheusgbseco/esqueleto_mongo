using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain.Model
{
    public class ConfigEmailBase
    {
		public string FakeEmail { get; set; }

		public string SmtpServer { get; set; }

		public string SmtpUser { get; set; }

		public string SmtpPassword { get; set; }

        public string FromEmail { get; set; }

        public string FromText { get; set; }

        public int SmtpPortNumber { get; set; }

		public string TextFormat { get; set; }

		public string ApiKey { get; set; }
	}
}
