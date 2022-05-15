using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SP.Portal.Common.WebApi;
using SP.Portal.Core.Receivers;

namespace SP.Portal.Common.Features.RegisterWebApiFeature
{
    [Guid("1f4edcb1-e1bd-4886-a72e-d24aeb3ddda9")]
    public class RegisterWebApiFeatureEventReceiver : WebConfigEngineFeatureReceiver
    {
        protected override string OwnerModif => "WebApiStart";

        protected override void InitializeModificationEntries()
        {
            Entries.Clear();

            UpdateModuleNode();
        }

        private void UpdateModuleNode()
        {
            var entry = new ModificationEntry
            {
                Name = $"add[@name=\"WebApiStartModule\"]",
                XPath = "/configuration/system.webServer/modules",
                Value = $"<add name=\"WebApiStartModule\" type=\"{typeof(WebApiStartModule).AssemblyQualifiedName}\" />",
                Sequence = 0,
                ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            };
            Entries.Add(entry);

            //<add name="ApiURIs-ISAPI-Integrated-4.0" path="/_odata/*" verb="GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />

            var webApiHandlerName = "WebApiURIs-ISAPI-Integrated-4.0";
            var webApiHandlerPath = "/_webapi/*";
            var entry1 = new ModificationEntry
            {
                Name = $"add[@name=\"{webApiHandlerName}\"]",
                XPath = "/configuration/system.webServer/handlers",
                Value = $"<add name=\"{webApiHandlerName}\" path=\"{webApiHandlerPath}\" verb=\"GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS\" type=\"System.Web.Handlers.TransferRequestHandler\" preCondition=\"integratedMode,runtimeVersionv4.0\"/>",
                Sequence = 0,
                ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            };
            Entries.Add(entry1);

            var entry2 = new ModificationEntry
            {
                Name = $"*[local-name()='dependentAssembly'][*/@name='System.Web.Http'][*/@publicKeyToken='31bf3856ad364e35'][*/@culture='neutral']",
                XPath = "configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']",
                Value = $"<dependentAssembly><assemblyIdentity name='System.Web.Http' publicKeyToken='31bf3856ad364e35' culture='neutral' /><bindingRedirect oldVersion='0.0.0.0-5.2.3.0' newVersion='5.2.3.0' /></dependentAssembly>",
                Sequence = 0,
                ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            };
            Entries.Add(entry2);

            var entry3 = new ModificationEntry
            {
                Name = $"*[local-name()='dependentAssembly'][*/@name='System.Net.Http.Formatting'][*/@publicKeyToken='31bf3856ad364e35'][*/@culture='neutral']",
                XPath = "configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']",
                Value = $"<dependentAssembly><assemblyIdentity name='System.Net.Http.Formatting' publicKeyToken='31bf3856ad364e35' culture='neutral' /><bindingRedirect oldVersion='0.0.0.0-5.2.3.0' newVersion='5.2.3.0' /></dependentAssembly>",
                Sequence = 0,
                ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            };
            Entries.Add(entry3);

            //var entry5 = new ModificationEntry
            //{
            //    Name = $"*[local-name()='dependentAssembly'][*/@name='Autofac'][*/@publicKeyToken='17863af14b0044da'][*/@culture='neutral']",
            //    XPath = "configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']",
            //    Value = $"<dependentAssembly><assemblyIdentity name='Autofac' publicKeyToken='17863af14b0044da' culture='neutral' /><bindingRedirect oldVersion='0.0.0.0-4.6.1.0' newVersion='4.6.1.0' /></dependentAssembly>",
            //    Sequence = 0,
            //    ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            //};
            //Entries.Add(entry5);

            //var entry4 = new ModificationEntry
            //{
            //    Name = $"*[local-name()='dependentAssembly'][*/@name='Newtonsoft.Json'][*/@publicKeyToken='30ad4fe6b2a6aeed'][*/@culture='neutral']",
            //    XPath = "configuration/runtime/*[local-name()='assemblyBinding' and namespace-uri()='urn:schemas-microsoft-com:asm.v1']",
            //    Value = $"<dependentAssembly><assemblyIdentity name='Newtonsoft.Json' publicKeyToken='30ad4fe6b2a6aeed' culture='neutral' /><bindingRedirect oldVersion='0.0.0.0-10.0.0.0' newVersion='10.0.0.0' /></dependentAssembly>",
            //    Sequence = 0,
            //    ModType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            //};
            //Entries.Add(entry4);
        }
    }
}
