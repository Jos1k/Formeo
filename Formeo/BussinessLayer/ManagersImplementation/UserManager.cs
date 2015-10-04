using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	class UserManager : IUserManager
	{

		public RoleManager<IdentityRole> RoleManager
		{
			get
			{
				return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
			}
		}

		public ApplicationDbContext CurrentDbContext { get { return new ApplicationDbContext(); } }

		public ApplicationUser GetCurrentUser()
		{
			ApplicationDbContext currentContext = new ApplicationDbContext();

			string Id = HttpContext.Current.User.Identity.GetUserId();

			var currentUser = currentContext.Users.Where(user => user.Id == Id).FirstOrDefault();
			return currentUser;
		}

		public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
		{
			var role = RoleManager.FindByName(roleName);
			var a = CurrentDbContext.Users.Where(x => x.Roles.FirstOrDefault().RoleId == role.Id).ToList();
			return a;
		}

		public async Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(string roleName)
		{
			var role = await RoleManager.FindByNameAsync(roleName);
			return CurrentDbContext.Users.Where(x => x.Roles.First().RoleId == role.Id).ToList();
		}

	}
}
