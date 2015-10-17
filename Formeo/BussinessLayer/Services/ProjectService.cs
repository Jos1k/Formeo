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

		public string GetProjectsByCreatorUserJSON(string creatorUserId, Formeo.Models.StaticData.OrderStatusEnum orderStatus)
		{
			IEnumerable<Project> projects =
				_projectsManager
				.GetProjectByCreator(creatorUserId, orderStatus);

			var projectsShort = GetProjectShort(projects);

			return JsonConvert.SerializeObject(projectsShort);
		}


		private IEnumerable<ProjectForCreatorShort> GetProjectShort(IEnumerable<Project> projects)
		{

			if (projects == null || projects.Count() == 0)
			{
				return new List<ProjectForCreatorShort>();
			}
			var projectsShort = projects.Select(project => new ProjectForCreatorShort()
			{
				Id = project.ID,
				Name = project.Name,
				Quantity = project.OverallQuantity,
			});
			return projectsShort;
		}

		public string GetProjectInfosForProducerJSON(long companyId, Formeo.Models.StaticData.PrintObjectStatusEnum poStatus)
		{
			IEnumerable<ProjectInfo> projectInfos = _projectsManager.GetProjectInfosForProducer(companyId, poStatus);
			var pinfosShort = projectInfos.Select(poinfo => ToProjectInfoForProducerShort(poinfo));
			return JsonConvert.SerializeObject(pinfosShort);
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

		private class ProjectForCreatorShort
		{
			public long Id { get; set; }
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
	}
}
