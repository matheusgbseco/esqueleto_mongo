using Common.Orm;
using Project.Core.Filters;
using Project.Core.Data.Model;
using System.Linq;
using Common.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Domain.Interfaces;

namespace Project.Core.Data.Repository
{
    public class ProdutoRepository : RepositoryMongoDb<Produto>
    {
        CategoriaRepository _categoriaRepository;
        public ProdutoRepository(IContextMongoDb databaseConfig, CategoriaRepository categoriaRepository) : base(databaseConfig)
        {
            this._categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Produto>> GetByFilters(ProdutoFilter filters)
        {
            var querybase = this.GetAll();

            var queryFilter = this.SimpleFilters(filters, querybase);

            return queryFilter;
        }

        public async Task<Produto> GetById(ProdutoFilter filters)
        {
            var querybase = await this.GetByFilters(filters);
            return querybase.SingleOrDefault();
        }

        public Produto GetByModel(Produto model)
        {
            return this.GetAll().Where(_ => _.ProdutoId == model.ProdutoId).FirstOrDefault();
        }

        public async Task<IEnumerable<dynamic>> GetGrid(ProdutoFilter filters)
        {
            var listResult = new List<dynamic>();
            var querybase = await this.GetByFilters(filters);

            foreach (var item in querybase)
            {
                var categoria = this._categoriaRepository.GetAll().Where(_ => _.CategoriaId == item.CategoriaId).FirstOrDefault();


                listResult.Add(new
                {
                    ProdutoId = item.ProdutoId,
                    Nome = item.Nome,
                    categoria = categoria.IsNotNull() ? categoria.Nome : default
                });
            }

            return listResult;
        }

        public async Task<IEnumerable<Produto>> GetDetails(ProdutoFilter filters)
        {
            var querybase = await this.GetByFilters(filters);
            return querybase.ToList();
        }

        protected IEnumerable<Produto> SimpleFilters(ProdutoFilter filters, IEnumerable<Produto> queryBase)
        {
            var queryFilter = queryBase;

            if (filters.ProdutoId.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.ProdutoId == filters.ProdutoId);
            }
            if (filters.Nome.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.Nome.ToLower().Contains(filters.Nome.ToLower()));
            }
            if (filters.CategoriaId.IsSent())
            {
                queryFilter = queryFilter.Where(_ => _.CategoriaId == filters.CategoriaId);
            }

            return queryFilter;
        }


    }
}
