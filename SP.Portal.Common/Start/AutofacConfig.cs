using Autofac;
using Autofac.Integration.WebApi;
using SP.Portal.Core.Data;
using SP.Portal.Core.Helpers;
using SP.Portal.Data;
using System.Web.Http;

namespace SP.Portal.Common.Start
{
    public class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {    // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);

            // регистрируем споставление типов
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
            builder.Register<IDbContext>(b => new MainBaseDbDataContext(SettingsHelper.GetConnectionString())).InstancePerLifetimeScope();

            // регистрируем репозитории работы с данными
            builder.RegisterModule<RepositoryRegistrationModule>();

            // регистрируем domain сервисы
            builder.RegisterModule<ServiceRegistrationModule>();

            // регистрируем конфигурацию AutoMapper
            //builder.RegisterModule<AutoMapperModule>();??

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();

            IoC.Container = container;

            // установка сопоставителя зависимостей
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
