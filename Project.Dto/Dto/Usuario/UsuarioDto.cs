using System;
using Common.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Core.Dto
{
	public class UsuarioDto : DtoBase
	{

        [BsonId]
        public ObjectId Id { get; set; }
        public string UsuarioId { get; set; } 
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; } 
		
	}
}