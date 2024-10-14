using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Abstractions
{
    public interface IBaseEntityRepository<TEntity>
    {
        Task<List<TEntity>> GetMany(int? skip = null, int? take = null);
        Task<TEntity> GetById(int id);
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Remove(int id);
    }
}
