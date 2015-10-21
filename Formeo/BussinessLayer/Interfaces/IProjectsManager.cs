﻿using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderStatus = Formeo.Models.StaticData.OrderStatusEnum;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IProjectsManager
	{
		Project CreateProject(
			string projectName,
			string userId,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo);

		IEnumerable<Project> GetProjectsByStatus(OrderStatus ordersStatus);
		IEnumerable<Project> GetProjectByCreatorCompany(long companyId, StaticData.OrderStatusEnum orderStatus);
		IEnumerable<ProjectInfo> GetProjectInfosForProducer(long companyId, StaticData.PrintObjectStatusEnum printObjectStatus);
		IEnumerable<ProjectInfo> GetProjectInfosByProjectId(long projectId);
		ProjectInfo GetProjectInfo(long projectId, long printObjectId);
		void SetPrintObjectStatus(long orderId, long printObjectId, StaticData.PrintObjectStatusEnum status);
	}
}
