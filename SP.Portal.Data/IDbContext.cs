using SP.Portal.Core.Entities.Base;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace SP.Portal.Data
{
    public interface IDbContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity, new();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void SetAsModified<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void ExecuteCommand(string commandText, params object[] parameters);

        ObjectResult<TResult> ExecuteQuery<TResult>(string commandText, params object[] parameters) where TResult : class;

        void BeginTransaction();

        int Commit();
        Task<int> CommitAsync();

        void Rollback();
    }
}
