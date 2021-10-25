using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Project.Core.Data.Model
{
    public class Produto
    {
        public Categoria Categoria { get; set; }
        public Produto(string nome,string categoriaId)
        {
            this.ProdutoId = Guid.NewGuid().ToString();
            this.Nome = nome;
            this.CategoriaId = categoriaId;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string ProdutoId { get; set; }
        public string CategoriaId { get; set; }
        public string Nome { get; set; }
    }
}
