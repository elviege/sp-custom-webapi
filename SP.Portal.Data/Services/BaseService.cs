using SP.Portal.Core.Data;
using SP.Portal.Core.Data.Repositories;
using SP.Portal.Core.Entities.Base;
using SP.Portal.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SP.Portal.Data.Services
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : IEntity, new()
    {
        protected IUnitOfWork _unitOfWork { get; private set; }
        protected readonly IRepository<TEntity> _repository;
        private bool _disposed;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<TEntity>();
        }

        public BaseService(IUnitOfWork unitOfWork, Type repositoryType)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<TEntity>(repositoryType);
        }

        public IRepository<TEntity> Repository => _repository;

        public TEntity Add(TEntity entity)
        {
            _repository.Add(entity);
            _unitOfWork.Commit();

            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _repository.Add(entity);
            await _unitOfWork.CommitAsync();

            return entity;
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
            _unitOfWork.Commit();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null || entities.Count() == 0) return;
            foreach (var entity in entities)
            {
                _repository.Update(entity);
            }
            _unitOfWork.Commit();
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public TEntity GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            return _unitOfWork.CommitAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _repository.Delete(entity);
            return _unitOfWork.CommitAsync();
        }

        public virtual IQueryable<TElement> QueryToCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TElement : class
        {
            return _repository.QueryToCollection(entity, navigationProperty);
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
                _unitOfWork.Dispose();
            }
            _disposed = true;
        }

    }
}
