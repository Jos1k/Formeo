using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Services
{
	public class ProjectService : IProjectService
	{
		private IUserManager _userManager;
		private IProjectsManager _projectsManager;
		IPrintObjectsManager _printObjectManager;
		public ProjectService(
			IUserManager userManager,
			IProjectsManager projectsManager,
			IPrintObjectsManager printObjectManager)
		{
			_userManager = userManager;
			_projectsManager = projectsManager;
			_printObjectManager = printObjectManager;
		}

		public string GetProjectsByCreatorCompanyJSON(long companyId, Formeo.Models.StaticData.OrderStatusEnum orderStatus)
		{
			IEnumerable<Project> projects =
				_projectsManager
				.GetProjectByCreatorCompany(companyId, orderStatus);

			var projectsShort = GetProjectsShort(projects);

			return JsonConvert.SerializeObject(projectsShort);
		}

		public string GetProjectInfosForProducerJSON(long companyId, Formeo.Models.StaticData.PrintObjectStatusEnum poStatus)
		{
			IEnumerable<ProjectInfo> projectInfos = _projectsManager.GetProjectInfosForProducer(companyId, poStatus);
			var pinfosShort = projectInfos.Select(poinfo => ToProjectInfoForProducerShort(poinfo));
			return JsonConvert.SerializeObject(pinfosShort);
		}

		public string GetProjectInfoDetailsForProducerJSON(long projectId, long printObjectId)
		{
			ProjectInfo projectInfo = _projectsManager.GetProjectInfo(projectId, printObjectId);

			ProjectInfoDetailsForCustomer printObjectDetails = new ProjectInfoDetailsForCustomer()
			{
				Address = projectInfo.Project.Address,
				ArtNo = projectInfo.PrintObject.ArticleNo,
				City = projectInfo.Project.City,
				Country = projectInfo.Project.Country,
				CompanyName = projectInfo.Project.CompanyCreator.Name,
				LastName = projectInfo.Project.LastName,
				OverralPrice = projectInfo.Price,
				PricePerItem = projectInfo.SelectedBidPrice,
				Currency = projectInfo.SelectedCurrency,
				PrintMaterial = projectInfo.PrintObject.PrintMaterial,
				PrintObjectId = projectInfo.PrintObject.ID,
				ProductName = projectInfo.PrintObject.Name,
				ProjectID = projectInfo.Project.ID,
				Quantity = projectInfo.Quantity,
				Surname = projectInfo.Project.Surname,
				ZipCode = projectInfo.Project.ZipCode
			};
			return JsonConvert.SerializeObject(printObjectDetails);
		}

		private IEnumerable<ProjectForCreatorShort> GetProjectsShort(IEnumerable<Project> projects)
		{

			if (projects == null || projects.Count() == 0)
			{
				return new List<ProjectForCreatorShort>();
			}
			var projectsShort = projects.Select(project => new ProjectForCreatorShort()
			{
				ProjectId = project.ID,
				Name = project.Name,
				Quantity = project.OverallQuantity,
			});
			return projectsShort;
		}

		public string GetProjectInfosByProjectIdJSON(long projectId)
		{
			IEnumerable<ProjectInfo> projectInfos = _projectsManager.GetProjectInfosByProjectId(projectId);
			var printObjectInfosShort = projectInfos.Select(ToProjectInfoForCustomerShort);
			return JsonConvert.SerializeObject(printObjectInfosShort);
		}
		private ProjectInfoForProducerShort ToProjectInfoForProducerShort(ProjectInfo projectInfo)
		{
			ProjectInfoForProducerShort pinfoShort = new ProjectInfoForProducerShort()
			{
				CompanyName = projectInfo.Project.CompanyCreator.Name,
				PrinObjectID = projectInfo.PrintObjectId,
				PrintObjectName = projectInfo.PrintObject.Name,
				ProjectId = projectInfo.ProjectId,
				Quantity = projectInfo.Quantity,
			};
			return pinfoShort;
		}
		private ProjectInfoForCustomerShort ToProjectInfoForCustomerShort(ProjectInfo projectInfo)
		{
			ProjectInfoForCustomerShort pinfoShort = new ProjectInfoForCustomerShort()
			{
				CompanyName = projectInfo.CompanyProducer.Name,
				PrinObjectID = projectInfo.PrintObjectId,
				PrintObjectName = projectInfo.PrintObject.Name,
				ArtNo = projectInfo.PrintObject.ArticleNo,
				ProjectId = projectInfo.ProjectId,
				Quantity = projectInfo.Quantity,
				Status = projectInfo.Status.GetPrintObjectStatusName()
			};
			return pinfoShort;
		}

		private class ProjectForCreatorShort
		{
			public long ProjectId { get; set; }
			public string Name { get; set; }
			public int Quantity { get; set; }

		}
		private class ProjectInfoForProducerShort
		{
			public long ProjectId { get; set; }
			public long PrinObjectID { get; set; }
			public string PrintObjectName { get; set; }
			public int Quantity { get; set; }
			public string CompanyName { get; set; }

		}
		private class ProjectInfoForCustomerShort
		{
			public long ProjectId { get; set; }
			public long PrinObjectID { get; set; }
			public string PrintObjectName { get; set; }
			public string ArtNo { get; set; }
			public int Quantity { get; set; }
			public string CompanyName { get; set; }
			public string Status { get; set; }
		}

		private class ProjectInfoDetailsForCustomer
		{
			public long PrintObjectId { get; set; }
			public long ProjectID { get; set; }
			public string ProductName { get; set; }
			public string ArtNo { get; set; }
			public int Quantity { get; set; }
			public string PrintMaterial { get; set; }
			public decimal PricePerItem { get; set; }
			public decimal OverralPrice { get; set; }

			#region DeliveryInfo

			public string Surname { get; set; }

			public string LastName { get; set; }

			public string Address { get; set; }

			public string ZipCode { get; set; }

			public string City { get; set; }
			public string Country { get; set; }

			#endregion

			public string CompanyName { get; set; }

			public string Currency { get; set; }
		}

	}
}
