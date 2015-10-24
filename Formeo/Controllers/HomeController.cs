using System.Net;
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
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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

		private async Task<IdentityResult> AddUserToRoles( FormeoRegisterViewModel model, ApplicationUser user ) {
			IdentityResult result = await UserManager.AddToRoleAsync( user.Id, model.selectedRole );

			return result;
		}

		private void AddErrors( IdentityResult result ) {
			foreach( var error in result.Errors ) {
				ModelState.AddModelError( "", error );
			}
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public async Task<ActionResult> RegisterFormeo( FormeoRegisterViewModel model ) {
			//todo: remove this stub company
			//Company comp = new ApplicationDbContext().Companies.First();
			if( ModelState.IsValid ) {
				try {
					var user = new ApplicationUser {
						UserName = model.username,
						Email = model.email,
						Adress = model.address,
						ZipCode = model.postal,
						City = model.city,
						Country = model.country,
						CompanyId = model.companyId
					};

					IdentityResult result = await UserManager.CreateAsync( user, model.password );
					if( result.Succeeded ) {
						result = await AddUserToRoles( model, user );
						if( result == null || !result.Succeeded ) {
							AddErrors( result );
						}
						return Json( _userService.GetUsersByIdJSON( user.Id ) );
					}
				} catch( Exception ex ) {
					throw;
				}
			}



			// If we got this far, something failed, redisplay form
			return RedirectToAction( "Index", "Home" );
		}


		[HttpPost]
		public ActionResult RemoveUser( string email ) {
			ApplicationUser user = UserManager.FindByEmail( email );
			if( user == null ) {
				return new HttpStatusCodeResult( HttpStatusCode.NotFound );
			}
			_userManager.RemoveUser( user.Id );
			return new HttpStatusCodeResult( HttpStatusCode.OK );
			//return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );

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

			viewModel.Orders_ActiveOrders =
				_projectService
				.GetProjectsByCreatorCompanyJSON(
				company.ID,
				Formeo.Models.StaticData.OrderStatusEnum.InProgress );

			viewModel.Orders_CompletedOrders =
				_projectService
				.GetProjectsByCreatorCompanyJSON(
				company.ID,
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

		[HttpGet]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult EditUserModal() {
			return PartialView( "~/Views/Account/_EditUser.cshtml" );
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult EditCompany( Company company ) {
			_companiesManager.UpdateCompany( company );
			return Json( _companiesService.GetCompanyJSON( company.ID ) );
		}

		public class ShortUser {
			public string Id { get; set; }
			public long? CompanyId { get; set; }
			[Required]
			[EmailAddress]
			public string Email { get; set; }
			[Required]
			public string City { get; set; }
			[Required]
			public string Country { get; set; }
			[Required]
			public string Postal { get; set; }
			[Required]
			public string Address { get; set; }
			[Required]
			public string SelectedRole { get; set; }
		}


		public RoleManager<IdentityRole> RoleManager {
			get {
				return new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( DbContext ) );
			}
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult EditUser( ShortUser user ) {
			if( ModelState.IsValid ) {

				ApplicationUser resultUser = _userManager.GetUserById( user.Id );
				resultUser.ZipCode = user.Postal;
				resultUser.Email = user.Email;
				resultUser.Adress = user.Address;
				resultUser.City = user.City;
				resultUser.Country = user.Country;
				resultUser.CompanyId = user.CompanyId;

				_userManager.UpdateUser( resultUser );

				var oldRole = RoleManager.FindById( resultUser.Roles.FirstOrDefault().RoleId );
				var newRole = RoleManager.FindByName( user.SelectedRole );
				//resultUser.Roles.FirstOrDefault().RoleId = role.Id;

				UserManager.RemoveFromRole( resultUser.Id, oldRole.Name );
				UserManager.AddToRole( resultUser.Id, user.SelectedRole );

				DbContext.SaveChanges();
				DbContext.Entry<ApplicationUser>( resultUser );

				var result = _userService.GetUsersByIdJSON( resultUser.Id );
				dynamic desearResult = JsonConvert.DeserializeObject( result );
				desearResult.SelectedRole = user.SelectedRole;
				result = JsonConvert.SerializeObject( desearResult );
				return Json( result );
			}
			return View( user );
		}

		[HttpPost]
		[Authorize( Roles = StaticData.RoleNames.Admin )]
		public ActionResult RemoveCompany( long companyId ) {
			ICollection<string> deletedUsers = _companiesManager.RemoveCompany( companyId );
			return Json( JsonConvert.SerializeObject( deletedUsers ) );
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