using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	class UserManager : IUserManager
	{
		public ApplicationUser GetCurrentUser()
		{
			ApplicationDbContext currentContext = new ApplicationDbContext();

			string Id = HttpContext.Current.User.Identity.GetUserId();

			var currentUser = currentContext.Users.Where(user => user.Id == Id).FirstOrDefault();
			return currentUser;
		}

	}
}
