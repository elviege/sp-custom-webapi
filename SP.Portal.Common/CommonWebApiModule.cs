using SP.Portal.Core;
using SP.Portal.Common.Start;
using System.Reflection;
using System.Web.Http;
using System.Web.OData.Builder;

namespace SP.Portal.Common
{
    public class CommonWebApiModule : IWebApiModule
    {
        public void Register(HttpConfiguration config)
        {
            AutofacConfig.Register(config);
        }

        public Assembly GetAssembly()
        {
            return GetType().Assembly;
        }

        public void BuildModel(ODataModelBuilder builder)
        {
            //builder.EntitySet<UnitEntity>("Unit"); //???
        }
    }
}
