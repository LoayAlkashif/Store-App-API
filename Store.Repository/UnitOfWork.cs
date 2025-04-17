using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core;
using Store.Core.Repositories.Contract;
using Store.Repository.Data.Context;
using Store.Repository.Repositories;

namespace Store.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;

        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            this.context = context;
            _repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
     

        IGenericRepository<TEntity, TKey> IUnitOfWork.Repository<TEntity, TKey>()
        {
            var type = typeof(TEntity).Name;
            
            if (!_repositories.ContainsKey(type))
            {

                var repository = new GenericRepository<TEntity, TKey>(context);
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity, TKey>;

        }
    }
}
