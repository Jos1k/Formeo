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

namespace Formeo.BussinessLayer.ManagersImplementation {
	class UserManager : IUserManager {
		ApplicationDbContext _dbcontext;

		public UserManager( ApplicationDbContext dbcontext ) {
			_dbcontext = dbcontext;
		}

		public RoleManager<IdentityRole> RoleManager {
			get {
				return new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( _dbcontext ) );
			}
		}

		public ApplicationUser GetCurrentUser() {

			string Id = HttpContext.Current.User.Identity.GetUserId();

			var currentUser = _dbcontext.Users.Where( user => user.Id == Id ).FirstOrDefault();
			return currentUser;
		}

		public IEnumerable<ApplicationUser> GetUsersByRole( string roleName ) {
			var role = RoleManager.FindByName( roleName );
			var a = _dbcontext.Users.Where( x => x.Roles.FirstOrDefault().RoleId == role.Id ).ToList();
			return a;
		}

		public async Task<ICollection<ApplicationUser>> GetUsersByRoleAsync( string roleName ) {
			var role = await RoleManager.FindByNameAsync( roleName );
			return _dbcontext.Users.Where( x => x.Roles.First().RoleId == role.Id ).ToList();
		}

		public ApplicationUser GetUserById( string userId ) {
			return _dbcontext.Users.Where( user => user.Id == userId ).First();
		}


		public bool UserIsInRole( string userId, string roleName ) {
			var userStore = new UserStore<ApplicationUser>( _dbcontext );
			var userManager = new UserManager<ApplicationUser>( userStore );

			return userManager.IsInRole( userId, roleName );
		}

	}
}
