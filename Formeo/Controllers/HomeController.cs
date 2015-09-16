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
using Formeo.BussinessLayer;

namespace Formeo.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{

		private ApplicationUserManager _userManager;

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
			string Id = User.Identity.GetUserId();
			string role = UserManager.GetRoles(Id).FirstOrDefault();
			switch (role)
			{
				case StaticData.RoleNames.Admin: return RedirectToAction("IndexAdmin");
				case StaticData.RoleNames.Customer: return RedirectToAction("IndexCustomer");
				case StaticData.RoleNames.Producer: return RedirectToAction("IndexProducer");
				default:
					return View();
			}
		}

		#region Indexes

		[Authorize(Roles=StaticData.RoleNames.Admin)]
		public ActionResult IndexAdmin()
		{
			_IndexAdminViewModel viewModel = new _IndexAdminViewModel();
			viewModel.Customers = UserHelper.Instance.GetUsersByRole(StaticData.RoleNames.Customer).ToList();
			viewModel.Producers = UserHelper.Instance.GetUsersByRole(StaticData.RoleNames.Producer).ToList();

			return View(viewModel);
		}

		[Authorize(Roles = StaticData.RoleNames.Customer)]
		public ActionResult IndexCustomer()
		{
			return View();
		}

		[Authorize(Roles = StaticData.RoleNames.Producer)]
		public ActionResult IndexProducer()
		{
			return View();
		}

		#endregion


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