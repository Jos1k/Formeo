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
				return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
		}

		public ActionResult Index()
		{
			string Id = User.Identity.GetUserId();

			var userCheck = UserManager.Users.Where(user => user.Id == Id).FirstOrDefault();

			if (userCheck == null)
			{
				return RedirectToAction("Login", "Account");
			}

			string role = UserManager.GetRoles(Id).FirstOrDefault();
			switch (role)
			{
				case StaticData.RoleNames.Admin: return RedirectToAction("IndexAdmin");
				case StaticData.RoleNames.Customer: return RedirectToAction("IndexCustomer");
				case StaticData.RoleNames.Producer: return RedirectToAction("IndexProducer");
				default:
					return RedirectToAction("Login", "Account");
			}
		}

		#region Indexes

		[Authorize(Roles = StaticData.RoleNames.Admin)]
		public ActionResult IndexAdmin()
		{
			_IndexAdminViewModel viewModel = new _IndexAdminViewModel();
			var Customers = UserHelper.Instance.GetUsersByRole(StaticData.RoleNames.Customer).ToList();
			var Producers = UserHelper.Instance.GetUsersByRole(StaticData.RoleNames.Producer).ToList();

			viewModel.CustomersJSON = GetJSONUsers(Customers, StaticData.RoleNames.Customer);
			viewModel.ProducersJSON = GetJSONUsers(Producers, StaticData.RoleNames.Producer);

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


		#region Helpers

		private string GetJSONUsers(List<ApplicationUser> users, string role)
		{

			var users_short = users.Select
				(user => new
					{
						UserName = user.UserName,
						Company = user.Company == null ? "<No Data>" : user.Company.Name,
						Email = string.IsNullOrWhiteSpace(user.Email) ? "<No Data>" : user.Email,
						Address = string.IsNullOrWhiteSpace(user.Adress) ? "<No Data>" : user.Adress,
						Postal = string.IsNullOrWhiteSpace(user.ZipCode) ? "<No Data>" : user.ZipCode,
						City = string.IsNullOrWhiteSpace(user.City) ? "<No Data>" : user.City,
						Country = string.IsNullOrWhiteSpace(user.Country) ? "<No Data>" : user.Country,
						IsProducer = role == StaticData.RoleNames.Producer,
						IsCustomer = role == StaticData.RoleNames.Customer,
						IsAdmin = role == StaticData.RoleNames.Admin
					}
				)
				.ToArray();
			return JsonConvert.SerializeObject(users_short);
		}

		#endregion
	}
}