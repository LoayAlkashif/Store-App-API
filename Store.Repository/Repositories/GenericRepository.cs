using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Specification;
using Store.Repository.Data.Context;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>) await context.Products.Include(p => p.Brand).Include(p => p.Type).ToListAsync();
            }
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Tkey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return  await context.Products.Include(p => p.Brand).Include(p => p.Type)
                                    .FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity;
            }
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
           context.Update(entity);
        }

        public void Update(TEntity entity)
        {
            context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity, Tkey> spec)
        {
            return SpecificationEvaluator<TEntity, Tkey>.GetQuery(context.Set<TEntity>().AsQueryable(), spec);
        }

        public Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }
    }
}
