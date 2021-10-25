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
    public class ProdutoService
    {
        ProdutoRepository _rep;
        IMapper _mapper;
        public ProdutoService(ProdutoRepository rep, ValidationContract validation, CurrentUser user, IMapper mapper)
        {
            this._mapper = mapper;
            this._rep = rep;

        }


        public virtual async Task<ProdutoDtoSave> Save(ProdutoDtoSave model)
        {
            var obj = new Produto(model.Nome,model.CategoriaId);
            this._rep.Add(obj);
            return model;
        }
        public virtual async Task<Produto> SavePartial(ProdutoDtoSave dto)
        {
            var old = this._rep.GetAll().Where(_ => _.ProdutoId == dto.ProdutoId).FirstOrDefault();

            var model = new Produto(dto.Nome,dto.CategoriaId)
            {
                ProdutoId = dto.ProdutoId,
                Id = old.Id
            };

            Expression<Func<Produto, bool>> filter = x => x.Id.Equals(old.Id);
            var result = this._rep.Update(filter, model);

            return result;
        }


        public virtual async Task<ProdutoDto> Remove(ProdutoDto dto)
        {
            var delete = this._rep.GetAll().Where(_ => _.ProdutoId == dto.ProdutoId).FirstOrDefault();
            Expression<Func<Produto, bool>> filter = x => x.Id.Equals(delete.Id);
            this._rep.Remove(filter);

            return dto;
        }

    }
}
