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

			Database.SetInitializer(new TestInitializer());

			//DependencyLoader.Start();

			//check if DB is built
			var a = new ApplicationDbContext().Roles.ToArray();
		}
	}
}
