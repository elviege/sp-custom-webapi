using SP.Portal.Core.Data.Repositories;
using SP.Portal.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace SP.Portal.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly IDbContext _context;
        protected readonly IDbSet<TEntity> DbEntitySet;
        private bool _disposed;

        public BaseRepository(IDbContext context)
        {
            _context = context;
            DbEntitySet = _context.Set<TEntity>();
        }

        public IDbContext GetContext()
        {
            return _context; 
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbEntitySet.AsQueryable();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbEntitySet.FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual TEntity GetById(Guid id)
        {
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var entity = DbEntitySet.FirstOrDefault(t => t.Id == id);
                scope.Complete();
                return entity;
            }
        }

        public virtual TEntity GetByFilter(Func<TEntity, bool> filter)
        {
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var entity = DbEntitySet.FirstOrDefault(filter);
                scope.Complete();
                return entity;
            }
        }

        public virtual void Add(TEntity entity)
        {
            DbEntitySet.AddOrUpdate(entity);
            //_context.SetAsAdded(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbEntitySet.AddOrUpdate(entity);
            //_context.SetAsModified(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            DbEntitySet.AddOrUpdate(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.SetAsDeleted(GetById(entity.Id) ?? entity);
        }

        public virtual void ExecuteCommand(string commandText, params object[] parameters)
        {
            _context.ExecuteCommand(commandText, parameters);
        }

        public virtual ObjectResult<TResult> ExecuteQuery<TResult>(string commandText, params object[] parameters) where TResult : class
        {
            return _context.ExecuteQuery<TResult>(commandText, parameters);
        }

        public virtual IQueryable<TElement> QueryToCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TElement : class
        {
            return _context.Entry(entity).Collection(navigationProperty).Query();
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
            }
            _disposed = true;
        }
    }
}
