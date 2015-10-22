﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Formeo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using Formeo.BussinessLayer.ManagersImplementation;
using Formeo.BussinessLayer.Interfaces;

namespace Formeo.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext _applicationContext;

		public ApplicationDbContext ApplicationContext
		{
			get
			{
				return _applicationContext ?? (_applicationContext = new ApplicationDbContext());
			}
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

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

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = UserManager.FindByEmail(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Invalid login attempt.");
				return View(model);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
					return RedirectToAction("Index", "Home");
				}

				AddErrors(result);
			}
			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpPost]
		public ActionResult RemoveUser(string userName)
		{
			ApplicationUser user = UserManager.FindByName(userName);
			if (user == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}

			IdentityResult removeResult = UserManager.Delete(user);

			if (removeResult.Succeeded)
			{
				return new HttpStatusCodeResult(HttpStatusCode.OK);
			}
			return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

		}

		[HttpPost]
		public ActionResult UpdateUserInfo(string userName, string field, string newValue)
		{

			ApplicationUser userToUpdate = ApplicationContext.Users.Where(user => user.UserName == userName).FirstOrDefault();
			if (userToUpdate == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}

			switch (field)
			{
				case "email":
					{
						userToUpdate.Email = newValue;
						break;
					}
				case "address":
					{
						userToUpdate.Adress = newValue;
						break;
					}
				case "postal":
					{
						userToUpdate.ZipCode = newValue;
						break;
					}
				case "city":
					{
						userToUpdate.City = newValue;
						break;
					}
				case "country":
					{
						userToUpdate.Country = newValue;
						break;
					}

				default:
					return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
			}
			try
			{
				
				ApplicationContext.SaveChanges();
				return new HttpStatusCodeResult(HttpStatusCode.OK);
			}
			catch
			{
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
			}
		}

		private async Task<IdentityResult> AddUserToRoles(FormeoRegisterViewModel model, ApplicationUser user)
		{
			IdentityResult result = await UserManager.AddToRoleAsync(user.Id, model.selectedRole);

			return result;
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Login", "Account");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}