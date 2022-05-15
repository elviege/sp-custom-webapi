using SP.Portal.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SP.Portal.Core.Data.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IEntity, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(Guid id);

        TEntity GetById(Guid id);

        TEntity GetByFilter(Func<TEntity, bool> filter);

        void Add(TEntity entity);

        void Update(TEntity entity);
        void AddOrUpdate(TEntity entity);

        void Delete(TEntity entity);

        void ExecuteCommand(string commandText, params object[] parameters);

        ObjectResult<TResult> ExecuteQuery<TResult>(string commandText, params object[] parameters) where TResult : class;
        IQueryable<TElement> QueryToCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TElement : class;
    }
}
