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

namespace Project.Core.Services
{
    public class UsuarioService 
    {
        public UsuarioService(UsuarioRepository rep, ValidationContract validation, CurrentUser user, IMapper mapper)
        { }


        public virtual async Task<dynamic> CriarUsuario(UsuarioDtoSave model)
        {
           

            return model;
        }
    }
}
