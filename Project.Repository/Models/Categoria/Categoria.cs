using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Project.Core.Data.Model
{
    public class Categoria
    {
        public Categoria(string nome)
        {
            this.CategoriaId = Guid.NewGuid().ToString();
            this.Nome = nome;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string CategoriaId { get; set; }
        public string Nome { get; set; }
    }
}
