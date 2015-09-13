using Formeo.EFInfrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Formeo.Models;

namespace Formeo
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Database.SetInitializer<FormeoDBContext>(new DropCreateDatabaseIfModelChanges<FormeoDBContext>());
            //var a = new FormeoDBContext().Project.ToArray();
            var a = new ApplicationDbContext().Projects.ToArray();
            //Database.SetInitializer(new TestInitializer());
        }
	}
}
