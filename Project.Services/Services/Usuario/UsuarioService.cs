using Common.Domain.Interfaces;
using Common.Validation;
using System.Threading.Tasks;
using System.Collections.Generic;
using Project.Core.Dto;
using Project.Core.Data.Repository;
using Project.Core.Data.Model;
using Common.Domain.Model;
using AutoMapper;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Common.Domain.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Project.Core.Services
{
    public class UsuarioService
    {
        UsuarioRepository _rep;
        IMapper _mapper;
        public UsuarioService(UsuarioRepository rep, ValidationContract validation, CurrentUser user, IMapper mapper)
        {
            this._mapper = mapper;
            this._rep = rep;

        }


        public virtual async Task<UsuarioDtoSave> CriarUsuario(UsuarioDtoSave model)
        {
            var usuarioObj = new Usuario(model.Nome, model.Login, model.Senha);
            this._rep.Add(usuarioObj);
            return model;
        }

        public async Task<Usuario> LoginChallenger(UsuarioDtoResult dto, SigningConfigurations signingConfigurations)
        {
            var result = this._rep.GetAll()
                .Where(_ => _.Login == dto.Login)
                .Where(_ => _.Senha == dto.Senha)
                .SingleOrDefault();

            if (result.IsNotNull())
            {
                result.Token = GenerateToken(result, signingConfigurations);
            }

            return result;
        }

        private string GenerateToken(Usuario usuario, SigningConfigurations signingConfigurations)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim("sub", usuario.UsuarioId.ToString()),
                }),
                //Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = signingConfigurations.SigningCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
