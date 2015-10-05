using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IProjectsManager
	{
		Project CreateProject(
			string projectName,
			int articleNo,
			string userId,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo);

		IEnumerable<Project> GetProjectsByUserId(string customerId, bool isCompleted);
		IEnumerable<Project> GetAllProjectsByUserId(string userId);
	}
}
