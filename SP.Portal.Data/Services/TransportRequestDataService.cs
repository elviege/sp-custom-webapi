using SP.Portal.Core.Data;
using SP.Portal.Core.Entities;
using System.Linq;

namespace SP.Portal.Data.Services
{
    public class TransportRequestDataService : BaseService<TransportRequestEntity>
    {
        public TransportRequestDataService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<TransportRequestEntity> GetAllRequests()
        {
            var result = Repository.GetAll().Where(e => e.Id != null);
            return result;
        }

        public int GetRequestsCount()
        {
            var result = Repository.GetAll().Where(e => e.Id != null).Count();
            return result;
        }
    }
}