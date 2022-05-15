using Microsoft.OData.Edm;
using SP.Portal.Common.Services;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace SP.Portal.Common.WebApi
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ModuleLoadService.Registration(config);

            // Web API configuration and services
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Services.Replace(typeof(IAssembliesResolver), new SPAssemblyResolver(ModuleLoadService.GetAssemblies()));
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new DefaultHttpControllerTypeResolver());

            HostingEnvironment.RegisterVirtualPathProvider(new WebAPIVirtualPathProvider());

            // Web API routes
            config.MapHttpAttributeRoutes(new DefaultDirectRouteProvider());

            config.MapODataServiceRoute(
                routeName: "OData",
                routePrefix: "_odata",
                model: GetModel(),
                batchHandler: new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));

            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "_webapi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // set OData 'Top' query param limits
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


            GlobalConfiguration.Configuration.EnsureInitialized();

        }

        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            foreach (var module in ModuleLoadService.GetModules())
            {
                module.BuildModel(builder);
            }

            builder.Namespace = "Service";

            return builder.GetEdmModel();
        }
    }

}
