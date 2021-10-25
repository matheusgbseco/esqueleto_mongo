using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Filters
{
    public class ProdutoFilter : Common.Domain.Base.FilterBase
    {
        public string ProdutoId { get; set; }
        public string CategoriaId { get; set; }
        public string Nome { get; set; }
    }
}
