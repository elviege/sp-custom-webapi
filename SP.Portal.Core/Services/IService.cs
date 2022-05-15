using SP.Portal.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SP.Portal.Core.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : IEntity, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(Guid id);

        TEntity GetById(Guid id);

        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        IQueryable<TElement> QueryToCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TElement : class;
    }
}
