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
    public class CategoriaRepository : RepositoryMongoDb<Categoria>
    {

        public CategoriaRepository(IContextMongoDb databaseConfig) : base(databaseConfig)
        {
        }

        public async Task<IEnumerable<Categoria>> GetByFilters(CategoriaFilter filters)
        {
            var querybase = this.GetAll();

            var queryFilter = this.SimpleFilters(filters, querybase);

            return queryFilter;
        }

        public async Task<Categoria> GetById(CategoriaFilter filters)
        {
            var querybase = await this.GetByFilters(filters);
            return querybase.SingleOrDefault();
        }

		public Categoria GetByModel(Categoria model)
        {
            return this.GetAll().Where(_ => _.CategoriaId == model.CategoriaId).FirstOrDefault();
        }

        public virtual async Task<IEnumerable<dynamic>> GetDataItems(CategoriaFilter filters)
        {
            var querybase = await this.GetByFilters(filters);
            return querybase.Select(_ => new { Id = _.CategoriaId, Name = _.Nome }).ToList();
        }
        protected IEnumerable<Categoria> SimpleFilters(CategoriaFilter filters, IEnumerable<Categoria> queryBase)
        {
            var queryFilter = queryBase;

            if (filters.CategoriaId.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.CategoriaId == filters.CategoriaId);
            }
            if (filters.Nome.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.Nome.ToLower().Contains(filters.Nome.ToLower()));
            }

            return queryFilter;
        }


    }
}
