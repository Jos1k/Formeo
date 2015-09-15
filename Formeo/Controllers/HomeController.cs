using Formeo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Formeo;
using Microsoft.AspNet.Identity.Owin;

namespace Formeo.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private  ApplicationUserManager  _userManager;

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		public ActionResult Index()
		{
			var Id = User.Identity.GetUserId();
            var a = UserManager.GetRoles(Id);
            switch (a[0])
                {
                    case "Admin": return View("IndexAdmin");
                    case "Customer": return View("IndexCustomer");
                    case "Producer": return View("IndexProducer");
                }
			return View();
		}


		[Authorize(Roles = "Customer")]
		public ActionResult About()
		{

			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}