using Autofac;
using SP.Portal.Core.Data.Repositories;
using SP.Portal.Data.Repositories;
using System.Linq;

namespace SP.Portal.Common.Start
{
    public class RepositoryRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BaseRepository<>).Assembly)
                .Where(_ => _.Name.EndsWith("Repository"))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IRepository<>)).InstancePerDependency();

        }
    }
}
