using SP.Portal.Core.Data;
using SP.Portal.Core.Data.Repositories;
using SP.Portal.Core.Entities.Base;
using SP.Portal.Data.Repositories;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace SP.Portal.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private bool _disposed;
        protected Hashtable Repositories;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity, new()
        {
            if (Repositories == null)
            {
                Repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(BaseRepository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));

            return (IRepository<TEntity>)Repositories[type];
        }

        public IRepository<TEntity> Repository<TEntity>(Type trepo) where TEntity : IEntity, new()
        {
            if (Repositories == null)
            {
                Repositories = new Hashtable();
            }

            var repositoryType = trepo;
            string typeName = repositoryType.Name;

            if (Repositories.ContainsKey(typeName))
            {
                return (IRepository<TEntity>)Repositories[typeName];
            }

            Repositories.Add(typeName, Activator.CreateInstance(repositoryType, _context));
            return (IRepository<TEntity>)Repositories[typeName];
        }

        public TRepo Repository<TRepo, TEntity>() where TRepo : IRepository<TEntity>
            where TEntity : IEntity, new()
        {
            if (Repositories == null)
            {
                Repositories = new Hashtable();
            }

            var repositoryType = typeof(TRepo);
            string typeName = repositoryType.Name;

            if (Repositories.ContainsKey(typeName))
            {
                return (TRepo)Repositories[typeName];
            }

            Repositories.Add(typeName, Activator.CreateInstance(repositoryType, _context));
            return (TRepo)Repositories[typeName];
        }


        public void BeginTransaction()
        {
            _context.BeginTransaction();
        }

        public int Commit()
        {
            return _context.Commit();
        }

        public Task<int> CommitAsync()
        {
            return _context.CommitAsync();
        }

        public void Rollback()
        {
            _context.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                if (Repositories != null)
                {
                    foreach (IDisposable repository in Repositories.Values)
                    {
                        repository.Dispose();
                    }
                }
            }
            _disposed = true;
        }
    }
}
