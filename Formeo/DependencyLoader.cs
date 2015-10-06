using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.BussinessLayer.ManagersImplementation;
using Formeo.BussinessLayer.Services;
using Formeo.Controllers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Formeo.Models;

namespace Formeo
{
	public static class DependencyLoader
	{

		public static void Start()
		{
			var container = BuildUnityContainer();
		DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
		private static UnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			//hack - don't touch. Look here for more info: http://mvchosting.asphostcentral.com/post/ASPNET-MVC-3-Hosting-Problem-in-implementing-IControllerActivator-in-ASPNET-MVC-3.aspx
			container.RegisterType<HomeController>(); 

			container.RegisterType<IPrintObjectsManager, PrintObjectsManager>();
			container.RegisterType<IUserManager, UserManager>();
			container.RegisterType<IProjectsManager, ProjectsManager>();

			container.RegisterType<IPrintObjectService, PrintObjectService>();
			container.RegisterType<IUserService, UserService>();
			container.RegisterType<IProjectService, ProjectService>();

			container.RegisterType<ApplicationDbContext>( new PerThreadLifetimeManager() );

			return container;
		}
	}
}