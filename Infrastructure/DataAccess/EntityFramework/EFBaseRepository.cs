using Domain.Core.BaseEntities;
using Infrastructure.AppContext;
using Infrastructure.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework
{
    public class EFBaseRepository<TEntity>:IRepository,IAsyncRepository,IAsyncInsertable<TEntity>,IAsyncUpdatableRepository<TEntity>,IAsyncDeletableRepository<TEntity>,IAsyncFindable<TEntity>,IAsyncQueryableRepository<TEntity>,IAsyncOrderableRepository<TEntity> where TEntity:BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _table;
        
        public EFBaseRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _table.AddAsync(entity);
            return entry.Entity;
        }

        public  async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public async Task<bool> AnysAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression is null ? await GetAllActives().AnyAsync() : await GetAllActives().AnyAsync(expression);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.FromResult(_table.Remove(entity));
        }

        public  async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public  async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true)
        {
            return await GetAllActives(tracking).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            return await GetAllActives(tracking).Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool orderByDesc, bool tracking = true)
        {
            return orderByDesc ? await GetAllActives(tracking).OrderByDescending(orderBy).ToListAsync() : await GetAllActives(tracking).OrderBy(orderBy).ToListAsync();
        }

        public  async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, bool orderByDesc,bool tracking = true)
        {
            var values = GetAllActives(tracking).Where(expression);
            return orderByDesc ? await values.OrderByDescending(orderBy).ToListAsync() : await values.OrderBy(orderBy).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(x => x.Id == id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public  async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.FromResult(_table.Update(entity).Entity);
        }
        protected IQueryable<TEntity> GetAllActives(bool tracking = true)
        {
            var values = _table.Where(x => x.Status != Domain.Enums.Status.Deleted);
            return tracking ? values : values.AsNoTracking();
        }
    }
}

