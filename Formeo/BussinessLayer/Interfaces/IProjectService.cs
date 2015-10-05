using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IProjectService
	{
		string GetProjectsByUserJSON(string userId, bool isCompleted);
	}
}
