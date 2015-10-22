using Formeo.Controllers.CustomAttributes;
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
using System.IO;

namespace Formeo.Controllers {
	[Authorize]
	public class HomeController : Controller {

		private IPrintObjectsService _printObjectService;
		private IUserService _userService;
		private ICompaniesService _companiesService;
		private IUserManager _userManager;
		private IProjectService _projectService;
		private ICompaniesManager _companiesManager;
		private IPrintObjectsManager _printObjectManager;

		[InjectionConstructor]
		public HomeController(
			IPrintObjectsService printObjectService,
			IUserService userService,
			IUserManager userManager,
			IProjectService projectService,
			ICompaniesManager companiesManager,
			IPrintObjectsManager printObjectManager,
			ICompaniesService companiesService ) {
			_printObjectService = printObjectService;
			_userService = userService;
			_userManager = userManager;
			_projectService = projectService;
			_companiesManager = companiesManager;
			_printObjectManager = printObjectManager;
			_companiesService = companiesService;
		}


		public ApplicationUserManager UserManager {
			get {
				return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
		}

		public ApplicationDbContext DbContext { get { return new ApplicationDbContext(); } }

		public ActionResult Index() {
			var userCheck = _userManager.GetCurrentUser();

			if( userCheck == null ) {
				return RedirectToAction( "Login", "Account" );
			}

			string role = UserManager.GetRoles( userCheck.Id ).FirstOrDefault();

			switch( role ) {
				case StaticData.RoleNames.Admin: return RedirectToAction( "IndexAdmin" );
				case StaticData.RoleNames.Customer: return RedirectToAction( "IndexCustomer" );
				case StaticData.RoleNames.Producer: return RedirectToAction( "IndexProducer" );
				default:
					return RedirectToAction( "Login", "Account" );
			}
		}


		#region Indexes

		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult IndexAdmin() {
			_IndexAdminViewModel viewModel = GetAdminHomepageViewModel();

			return View( viewModel );
		}


		[Authorize( Roles = StaticData.RoleNames.Customer )]
		public ActionResult IndexCustomer() {

			var currentUser = _userManager.GetCurrentUser();
			if( currentUser == null ) {
				return RedirectToAction( "Login", "Account" );
			}

			_IndexCustomerViewModel viewModel = new _IndexCustomerViewModel();

			var company = _companiesManager.GetCompanyByUserId( currentUser.Id );

			viewModel.PrintObjectsJSON = _printObjectService.GetPrintObjectsByCompanyCreatorJSON( company.ID );

			viewModel.ActiveProjectsJSON =
				_projectService
				.GetProjectsByCreatorUserJSON(
				currentUser.Id,
				Formeo.Models.StaticData.OrderStatusEnum.InProgress );

			viewModel.CompletedProjectsJSON =
				_projectService
				.GetProjectsByCreatorUserJSON(
				currentUser.Id,
				Formeo.Models.StaticData.OrderStatusEnum.Delivered );

			return View( viewModel );
		}

		[Authorize( Roles = StaticData.RoleNames.Producer )]
		public ActionResult IndexProducer() {
			var currentUser = _userManager.GetCurrentUser();
			if( currentUser == null ) {
				return RedirectToAction( "Login", "Account" );
			}
			var company = _companiesManager.GetCompanyByUserId( currentUser.Id );
			_IndexProducerViewModel viewModel = new _IndexProducerViewModel();

			viewModel.Dashboard_BidRequestedPrintObjects = _printObjectService.GetNeedBidPrintObjectsForProducerJSON( currentUser.Id, true );
			viewModel.Dashboard_PrintObjectsDelivered = _projectService.GetProjectInfosForProducerJSON( company.ID, StaticData.PrintObjectStatusEnum.Delivered );

			viewModel.Storage_PrintObjects = _printObjectService.GetPrintObjectsByCompanyCreatorJSON( company.ID );

			viewModel.Orders_OrdersInQueue = _projectService.GetProjectInfosForProducerJSON( company.ID, StaticData.PrintObjectStatusEnum.InQueue );
			viewModel.Orders_OrdersInProduction = _projectService.GetProjectInfosForProducerJSON( company.ID, StaticData.PrintObjectStatusEnum.Producing );

			return View( viewModel );
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult CreateCompany( Company company ) {
			long companyId = _companiesManager.CreateCompany( company );
			return Json( _companiesService.GetCompanyJSON( companyId ) );
		}

		[HttpGet]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult EditCompanyModal() {
			return PartialView( "~/Views/Company/_EditCompany.cshtml" );
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult EditCompany( Company company ) {
			_companiesManager.UpdateCompany( company );
			return Json( _companiesService.GetCompanyJSON( company.ID ) );
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult RemoveCompany( long companyId ) {
			ICollection<string> deletedUsers = _companiesManager.RemoveCompany( companyId );
			return Json( JsonConvert.SerializeObject(deletedUsers));
		}
		#endregion


		public ActionResult About() {

			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}
		#region Helpers
		private _IndexAdminViewModel GetAdminHomepageViewModel() {
			_IndexAdminViewModel viewModel;
			viewModel = new _IndexAdminViewModel();

			viewModel.CustomersJSON = _userService.GetUsersByRoleJSON( StaticData.RoleNames.Customer );
			viewModel.ProducersJSON = _userService.GetUsersByRoleJSON( StaticData.RoleNames.Producer );
			viewModel.CompaniesJSON = _companiesService.GetCompaniesJSON();
			return viewModel;
		}

		#endregion
	}
}