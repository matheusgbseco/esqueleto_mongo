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
    public class UsuarioRepository : RepositoryMongoDb<Usuario>
    {

        public UsuarioRepository(IContextMongoDb databaseConfig) : base(databaseConfig)
        {
        }

        public IEnumerable<Usuario> GetByFilters(UsuarioFilter filters)
        {
            var querybase = this.GetAll();

            var queryFilter = this.SimpleFilters(filters, querybase);

            return queryFilter;
        }

        public Usuario GetById(UsuarioFilter filters)
        {
            var querybase = this.GetByFilters(filters);
            return querybase.SingleOrDefault();
        }

		public Usuario GetByModel(Usuario model)
        {
            return this.GetAll().Where(_ => _.UsuarioId == model.UsuarioId).FirstOrDefault();
        }


        protected IEnumerable<Usuario> SimpleFilters(UsuarioFilter filters, IEnumerable<Usuario> queryBase)
        {
            var queryFilter = queryBase;

            if (filters.UsuarioId.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.UsuarioId == filters.UsuarioId);
            }
            if (filters.Nome.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.Nome.Contains(filters.Nome));
            }

            if (filters.Login.IsSent())
            {

                queryFilter = queryFilter.Where(_ => _.Login.Contains(filters.Login));
            }


            return queryFilter;
        }


    }
}
