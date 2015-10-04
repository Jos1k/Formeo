using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IUserManager
	{
		ApplicationUser GetCurrentUser();
		IEnumerable<ApplicationUser> GetUsersByRole(string roleName);
		Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(string roleName);
	}
}
