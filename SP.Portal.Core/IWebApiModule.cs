using System.Reflection;
using System.Web.Http;
using System.Web.OData.Builder;

namespace SP.Portal.Core
{
    public interface IWebApiModule
    {
        void Register(HttpConfiguration config);
        Assembly GetAssembly();
        void BuildModel(ODataModelBuilder builder);
    }

}
