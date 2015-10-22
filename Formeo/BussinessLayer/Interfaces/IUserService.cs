using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IUserService
	{
		string GetUsersByRoleJSON(string roleName);
		string GetUsersByIdJSON( string id );
	}
}
