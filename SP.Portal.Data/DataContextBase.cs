using SP.Portal.Core.Entities.Base;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace SP.Portal.Data
{
    public abstract class DataContextBase : DbContext, IDbContext
    {
        private ObjectContext _objectContext;
        private DbTransaction _transaction;

        protected DataContextBase(string connectionString) : base(connectionString) { }
        protected DataContextBase(DbConnection connection) : base(connection, true) { }

        #region IDbContext

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity, new()
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        public void ExecuteCommand(string commandText, params object[] parameters)
        {
            this._objectContext = ((IObjectContextAdapter)this).ObjectContext;
            if (_objectContext.Connection.State == ConnectionState.Closed)
            {
                _objectContext.Connection.Open();
            }

            _objectContext.CommandTimeout = 180;
            _objectContext.ExecuteStoreCommand(commandText, parameters);

        }

        public ObjectResult<TResult> ExecuteQuery<TResult>(string commandText, params object[] parameters) where TResult : class
        {
            this._objectContext = ((IObjectContextAdapter)this).ObjectContext;
            if (_objectContext.Connection.State == ConnectionState.Closed)
            {
                _objectContext.Connection.Open();
            }
            return _objectContext.ExecuteStoreQuery<TResult>(commandText, parameters);
        }

        public void BeginTransaction()
        {
            this._objectContext = ((IObjectContextAdapter)this).ObjectContext;
            if (_objectContext.Connection.State == ConnectionState.Closed)
            {
                _objectContext.Connection.Open();
                // return;
            }

            _transaction = _objectContext.Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        public int Commit()
        {
            try
            {
                BeginTransaction();
                var saveChanges = SaveChanges();
                _transaction.Commit();

                return saveChanges;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
            //            finally
            //            {
            //                Dispose();
            //            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                BeginTransaction();
                var saveChangesAsync = await SaveChangesAsync();
                _transaction.Commit();

                return saveChangesAsync;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class, IEntity, new()
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }
    }
}
