using Autofac;
using SP.Portal.Common.Services;
using SP.Portal.Core.Services;
using SP.Portal.Data.Services;
using System.Linq;

namespace SP.Portal.Common.Start
{
    public class ServiceRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BaseService<>).Assembly)
               .Where(_ => _.Name.EndsWith("Service"))
               .PropertiesAutowired()
               .AsSelf()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            //builder.RegisterType<SearchService>().InstancePerLifetimeScope();

            /*builder.RegisterAssemblyTypes(typeof(DtoBaseService<,>).Assembly)
                .Where(_ => _.Name.EndsWith("Service"))
                .PropertiesAutowired()
                .InstancePerLifetimeScope();*/

            builder.RegisterGeneric(typeof(BaseService<>))
                .As(typeof(IService<>)).InstancePerDependency();

            /*builder.RegisterGeneric(typeof(DtoBaseService<,>))
                .As(typeof(IDtoService<>)).InstancePerDependency();*/
        }
    }
}
