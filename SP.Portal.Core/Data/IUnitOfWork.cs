using SP.Portal.Core.Data.Repositories;
using SP.Portal.Core.Entities.Base;
using System;
using System.Threading.Tasks;

namespace SP.Portal.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity, new();

        IRepository<TEntity> Repository<TEntity>(Type trepo) where TEntity : IEntity, new();

        TRepo Repository<TRepo, TEntity>()
            where TRepo : IRepository<TEntity>
            where TEntity : IEntity, new();

        void BeginTransaction();

        int Commit();

        Task<int> CommitAsync();

        void Rollback();

        void Dispose(bool disposing);
    }
}
