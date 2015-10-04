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
using Newtonsoft.Json;
using Microsoft.Practices.Unity;
using Formeo.BussinessLayer.Interfaces;

namespace Formeo.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private ApplicationUserManager _userManager;

		IPrintObjectService _printObjectService;
		IUserService _userService; 

		[InjectionConstructor]
		public HomeController(IPrintObjectService printObjectService, IUserService userService)
		{
			_printObjectService = printObjectService;
			_userService = userService;
		}


		public ApplicationUserManager UserManager
		{
			get
			{
				return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
		}

		public ApplicationDbContext DbContext { get { return new ApplicationDbContext(); } }

		public ActionResult Index()
		{
			var userCheck = GetCurrentUser();

			if (userCheck == null)
			{
				return RedirectToAction("Login", "Account");
			}

			string role = UserManager.GetRoles(userCheck.Id).FirstOrDefault();
			switch (role)
			{
				case StaticData.RoleNames.Admin: return RedirectToAction("IndexAdmin");
				case StaticData.RoleNames.Customer: return RedirectToAction("IndexCustomer");
				case StaticData.RoleNames.Producer: return RedirectToAction("IndexProducer");
				default:
					return RedirectToAction("Login", "Account");
			}
		}

		private ApplicationUser GetCurrentUser()
		{
			string Id = User.Identity.GetUserId();

			var userCheck = UserManager.Users.Where(user => user.Id == Id).FirstOrDefault();
			return userCheck;
		}

		#region Indexes

		[Authorize(Roles = StaticData.RoleNames.Admin)]
		public ActionResult IndexAdmin()
		{
			_IndexAdminViewModel viewModel = GetAdminHomepageViewModel();

			return View(viewModel);
		}

		
		[Authorize(Roles = StaticData.RoleNames.Customer)]
		public ActionResult IndexCustomer()
		{
			_IndexCustomerViewModel viewModel = new _IndexCustomerViewModel();

			var currentUser = GetCurrentUser();
			if (currentUser == null)
			{
				return RedirectToAction("Login", "Account");
			}

			viewModel.PrintObjectsJSON = _printObjectService.GetPrintObjectsForUserJSON(currentUser);

			return View(viewModel);
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


		#region Helpers
		private _IndexAdminViewModel GetAdminHomepageViewModel()
		{
			_IndexAdminViewModel viewModel;
			viewModel = new _IndexAdminViewModel();
			
			viewModel.CustomersJSON = _userService.GetUsersByRoleJSON(StaticData.RoleNames.Customer);
			viewModel.ProducersJSON = _userService.GetUsersByRoleJSON(StaticData.RoleNames.Producer);
			return viewModel;
		}

		#endregion
	}
}