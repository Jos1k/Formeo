using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderStatus = Formeo.Models.StaticData.OrderStatusEnum;
using PrintObjectStatusEnum =Formeo.Models.StaticData.PrintObjectStatusEnum;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IProjectService
	{
		string GetProjectsByCreatorCompanyJSON(long companyId, OrderStatus orderStatus);
		string GetProjectInfosForProducerJSON(long companyId, PrintObjectStatusEnum poStatus);
		string GetProjectInfosByProjectIdJSON(long projectId);
	}
}
