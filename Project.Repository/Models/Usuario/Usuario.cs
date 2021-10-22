using System;

namespace Project.Core.Data.Model
{
    public class Usuario
    {
        public Usuario(string nome, string login, string senha)
        {
            this.UsuarioId = Guid.NewGuid().ToString();
            this.Nome = nome;
            this.Login = login;
            this.Senha = senha;
        }


        public string UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
    }
}
