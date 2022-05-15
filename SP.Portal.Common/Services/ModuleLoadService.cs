using SP.Portal.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SP.Portal.Common.Services
{
    public class ModuleLoadService
    {
        private static Hashtable _assemblies;

        public static void Registration(HttpConfiguration config)
        {
            var modules = GetModules();
            foreach (var module in modules)
            {
                module.Register(config);
            }

        }

        public static Assembly[] GetAssemblies()
        {
            if (_assemblies == null)
            {
                _assemblies = new Hashtable();
                foreach (var asmb in ModuleAssemblies.Modules)
                {
                    _assemblies.Add(asmb.FullName, asmb);
                }
            }

            return _assemblies.Values.Cast<Assembly>().ToArray();
        }

        public static IWebApiModule[] GetModules()
        {
            var assms = GetAssemblies();

            var moduleTypes = assms.SelectMany(asm => asm.GetExportedTypes())
                .Where(type => !type.IsAbstract && !type.IsGenericParameter && typeof(IWebApiModule).IsAssignableFrom(type));

            return moduleTypes.Select(Activator.CreateInstance).Cast<IWebApiModule>().ToArray();
        }
    }

    public static class ModuleAssemblies
    {
        public static Assembly[] Modules
        {
            get
            {
                var assemblies = new List<Assembly>();
                var assemblyNames = new[]
                {
                    "SP.Portal.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bc7345d8b5d4601",
                };

                foreach (var name in assemblyNames)
                {
                    try
                    {
                        assemblies.Add(Assembly.Load(name));
                    }
                    catch { }
                }

                return assemblies.ToArray();
            }
        }
    }
}
