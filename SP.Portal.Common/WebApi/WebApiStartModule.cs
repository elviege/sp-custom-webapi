using Microsoft.SharePoint.Administration;
using SP.Portal.Common.Modules;
using System.Web.Http;

namespace SP.Portal.Common.WebApi
{
    public class WebApiStartModule : ApplicationStartHandler
    {
        protected override void OnStart()
        {
            SPDiagnosticsService.Local.WriteTrace(0,
                new SPDiagnosticsCategory("WebApiStartModule.OnStart", TraceSeverity.Medium, EventSeverity.Information),
                TraceSeverity.Medium, string.Format("WebApiStartModule: Attaching Hub and VirtualPathProvider"), null);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
