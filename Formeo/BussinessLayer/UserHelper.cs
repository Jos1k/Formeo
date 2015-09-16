using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Formeo.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Formeo.BussinessLayer
{
	public class UserHelper
	{
		private static UserHelper _instance;

		public static UserHelper Instance 
		{
			get 
			{
				return _instance ?? (_instance = new UserHelper());
			}
		}

		#region Members

		UserManager<ApplicationUser> _userManager;
		RoleManager<IdentityRole> _roleManager;

		#endregion

		#region Properties

		public UserManager<ApplicationUser> UserManager
		{
			get
			{
				return _userManager
					?? (_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())));
			}
			private set
			{
				_userManager = value;
			}
		}

		public RoleManager<IdentityRole> RoleManager
		{
			get
			{
				return _roleManager
					?? (_roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())));
			}
			private set
			{
				_roleManager = value;
			}
		}

		#endregion

		#region Public Methods

		public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
		{
			var role = RoleManager.FindByName(roleName);
			var a = UserManager.Users.Where(x => x.Roles.FirstOrDefault().RoleId == role.Id).ToList();
			return a;
		}

		public async Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(string roleName)
		{
			var role = await RoleManager.FindByNameAsync(roleName);
			return UserManager.Users.Where(x => x.Roles.First().RoleId == role.Id).ToList();
		}

		#endregion

	}
}