using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Live_Performance.Peristence.Oracle;
using Live_Performance.Persistence;
using Newtonsoft.Json;

namespace Live_Performance
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PersistenceInjector.Inject(new OracleRepositoryProvider());

            // Include 0, false, null, etc
            GlobalConfiguration.Configuration
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .DefaultValueHandling = DefaultValueHandling.Include;
        }
    }
}