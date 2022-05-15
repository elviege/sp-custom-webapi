using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace SP.Portal.Common.WebApi
{
    public class SPAssemblyResolver : DefaultAssembliesResolver
    {

        private readonly IEnumerable<Assembly> _assemblies;

        public SPAssemblyResolver(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }
        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly>()
            {
                Assembly.GetExecutingAssembly(),
                typeof(System.Web.OData.MetadataController).Assembly
            }.Union(_assemblies).ToList();

        }
    }
}
