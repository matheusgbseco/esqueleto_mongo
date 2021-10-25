using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Filters
{
    public class CategoriaFilter : Common.Domain.Base.FilterBase
    {
        public string CategoriaId { get; set; }
        public string Nome { get; set; }
    }
}
