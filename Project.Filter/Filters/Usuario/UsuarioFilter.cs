using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Filters
{
    public class UsuarioFilter : Common.Domain.Base.FilterBase
    {
        public string UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
    }
}
