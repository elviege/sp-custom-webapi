using System;
using System.Web.Hosting;

namespace SP.Portal.Common.WebApi
{
    class WebAPIVirtualPathProvider : VirtualPathProvider
    {
        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            return Previous.CombineVirtualPaths(basePath, relativePath);
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return Previous.CreateObjRef(requestedType);
        }
        /// <summary>
        /// This is the only method where we need to do something. Every request containing signalr and the evil character (~), we remove the evil character.
        /// </summary>
        /// <param name="virtualDir"></param>
        /// <returns></returns>
        public override bool DirectoryExists(string virtualDir)
        {
            //removing the evil character - otherwise the hell freezes and yeah, SharePoint.
            if (virtualDir != null && (virtualDir.Contains("_odata") || virtualDir.Contains("_webapi")))
            {
                return false;
                //string tmp = virtualDir.TrimStart('~');
                //return Previous.DirectoryExists(tmp);
            }
            try
            {
                return Previous.DirectoryExists(virtualDir);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool FileExists(string virtualPath)
        {
            return Previous.FileExists(virtualPath);
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath,
                                                                              System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override string GetCacheKey(string virtualPath)
        {
            return Previous.GetCacheKey(virtualPath);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return Previous.GetDirectory(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return Previous.GetFile(virtualPath);
        }

        public override string GetFileHash(string virtualPath, System.Collections.IEnumerable virtualPathDependencies)
        {
            return Previous.GetFileHash(virtualPath, virtualPathDependencies);
        }
    }
}
