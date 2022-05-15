using SP.Portal.Core.Entities.Base;
using SP.Portal.Core.Services;

namespace SP.Portal.Common.Services
{
    public class BaseModelService<TEntity> where TEntity : IEntity, new()
    {
        protected readonly IService<TEntity> _service;

        public BaseModelService(IService<TEntity> service)
        {
            _service = service;
        }        
    }
}
