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
using System.Linq.Expressions;

namespace Project.Core.Services
{
    public class CategoriaService
    {
        CategoriaRepository _rep;
        IMapper _mapper;
        public CategoriaService(CategoriaRepository rep, ValidationContract validation, CurrentUser user, IMapper mapper)
        {
            this._mapper = mapper;
            this._rep = rep;

        }


        public virtual async Task<CategoriaDtoSave> Save(CategoriaDtoSave model)
        {
            var obj = new Categoria(model.Nome);
            this._rep.Add(obj);
            return model;
        }
        public virtual async Task<Categoria> SavePartial(CategoriaDtoSave dto)
        {
            var model = this._mapper.Map<CategoriaDtoSave, Categoria>(dto);
            var old = this._rep.GetAll().Where(_ => _.CategoriaId == dto.CategoriaId).FirstOrDefault();
            model.Id = old.Id;

            Expression<Func<Categoria, bool>> filter = x => x.Id.Equals(old.Id);
            var result = this._rep.Update(filter, model);

            return result;
        }


        public virtual async Task<CategoriaDto> Remove(CategoriaDto dto)
        {
            var delete = this._rep.GetAll().Where(_ => _.CategoriaId == dto.CategoriaId).FirstOrDefault();
            Expression<Func<Categoria, bool>> filter = x => x.Id.Equals(delete.Id);
            this._rep.Remove(filter);

            return dto;
        }

    }
}
