using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderStatus = Formeo.Models.StaticData.OrderStatusEnum;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IProjectService
	{
		string GetProjectsByCreatorUserJSON(string customerId, OrderStatus orderStatus);

		string GetProjectsByCompanyJSON(long companyId, OrderStatus orderStatus);
	}
}
