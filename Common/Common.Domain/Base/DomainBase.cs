using Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain.Base
{
    public class DomainBase
    {
        [NotMapped]
        public string AttributeBehavior { get; set; }

    }
}
