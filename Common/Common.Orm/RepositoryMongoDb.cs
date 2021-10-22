using Common.Domain.Base;
using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Orm
{
    public class RepositoryMongoDb<T>
    {
        public readonly IMongoCollection<T> _collection;
        public RepositoryMongoDb(IContextMongoDb databaseConfig)
        {
            var className = typeof(T);
            var cliente = new MongoClient(databaseConfig.ConnectionString);
            var database = cliente.GetDatabase(databaseConfig.DatabaseName);

            _collection = database.GetCollection<T>(className.ToString());
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this._collection.Find(c => true).ToList();
        }
        public virtual T Add(T entity)
        {
            this._collection.InsertOne(entity);
            return entity;
        }

        public virtual T Update(string id, T entity)
        {
            this._collection.ReplaceOne(id,entity);
            return entity;
        }


        public virtual void Remove(string id, T entity)
        {
            //_collection.DeleteOne(entity);
        }


        public virtual async Task<PaginateResult<T1>> PagingAndOrdering<T1>(FilterBase filters, IQueryable<T1> queryFilter)
        {
            var orderedQuery = queryFilter.OrderByProperty(filters);
            return await this.Paging(filters, orderedQuery);
        }

        public virtual async Task<PaginateResult<T1>> Paging<T1>(FilterBase filters, IQueryable<T1> queryFilter)
        {
            var totalCount = await this.CountAsync(queryFilter);
            var paginateResult = await this.Paging(filters, queryFilter, totalCount).ToListAsync();

            return new PaginateResult<T1>
            {
                TotalCount = totalCount,
                PageSize = filters.PageSize,
                ResultPaginatedData = paginateResult,
                Source = queryFilter
            };
        }


        #region async

        public Task<List<T2>> ToListAsync<T2>(IQueryable<T2> source)
        {
            return source.ToListAsync();
        }

        public Task<T2> SingleOrDefaultAsync<T2>(IQueryable<T2> source)
        {
            return source.SingleOrDefaultAsync();
        }

        public Task<T2> FirstOrDefaultAsync<T2>(IQueryable<T2> source)
        {
            return source.FirstOrDefaultAsync();
        }

        public Task<int> CountAsync<T2>(IQueryable<T2> source)
        {
            return source.CountAsync();
        }

        public Task<decimal> SumAsync<T2>(IQueryable<T2> source, Expression<Func<T2, decimal>> selector)
        {
            return source.SumAsync(selector);
        }

        //public Task<int> CommitAsync()
        //{
        //    return this.ctx.SaveChangesAsync();
        //}

        //public int Commit()
        //{
        //    return this.ctx.SaveChanges();
        //}

        #endregion

        #region helpers

        private IQueryable<T2> Paging<T2>(FilterBase filter, IQueryable<T2> source, int totalCount)
        {
            if (filter.IsPagination)
            {
                var pageIndex = filter.PageIndex > 0 ? filter.PageIndex - 1 : 0;
                var pageSize = filter.PageSize;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return source.Skip(filter.PageSkipped).Take(pageSize);
            }

            return source;
        }


        #endregion

    }

}
