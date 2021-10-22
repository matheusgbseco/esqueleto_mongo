using System;
using Common.Dto;

namespace Project.Core.Dto
{
	public class UsuarioDto  : DtoBase
	{	
        public Guid UsuarioId { get; set; } 
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; } 
		
	}
}