using System.Web;

namespace SP.Portal.Common.Modules
{
    public abstract class ApplicationStartHandler : IHttpModule
    {
        private static readonly object _mutex = new object();
        private static bool _initialized = false;

        protected abstract void OnStart();

        public void Init(HttpApplication application)
        {
            if (!_initialized)
                lock (_mutex)
                    if (!_initialized)
                        Application_Start();
        }

        private void Application_Start()
        {
            _initialized = true;
            OnStart();
        }

        public void Dispose() { }
    }
}
