using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Infrastructure.Extensions;
using ArticlesManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using System.Xml.Linq;

namespace ArticlesManagement.Infrastructure.Repositories
{
    public class BaseEntityRepository<TEntity> : IBaseEntityRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseEntityRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();

        }


        public async Task<List<TEntity>> GetMany(int? skip = null, int? take = null)
        {

            var result = (await _dbSet
                .ToListAsync())
                .SkipIf(skip)
                .TakeIf(take != null, take.GetValueOrDefault())
                .ToList();

            return result;
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task<TEntity> Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Remove(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
