using System;
using Common.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Core.Dto
{
	public class ProdutoDto : DtoBase
	{

        [BsonId]
        public ObjectId Id { get; set; }
        public string ProdutoId { get; set; }
        public string CategoriaId { get; set; }
        public string Nome { get; set; }
		
	}
}