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

		public string GetProjectsByCompanyJSON(long companyId, StaticData.OrderStatusEnum orderStatus)
		{
			IEnumerable<Project> projects =
				   _projectsManager
				   .GetProjectsByCompany(companyId, orderStatus);

			var projectsShort = GetProjectShort(projects);

			return JsonConvert.SerializeObject(projectsShort);
		}

		public string GetProjectsByCreatorUserJSON(string creatorUserId, Formeo.Models.StaticData.OrderStatusEnum orderStatus)
		{
			IEnumerable<Project> projects =
				_projectsManager
				.GetProjectByCreator(creatorUserId, orderStatus);

			var projectsShort = GetProjectShort(projects);

			return JsonConvert.SerializeObject(projectsShort);
		}


		private IEnumerable<ProjectShort> GetProjectShort(IEnumerable<Project> projects)
		{

			if (projects == null || projects.Count() == 0)
			{
				return new List<ProjectShort>();
			}
			var projectsShort = projects.Select(project => new ProjectShort()
			{
				Id = project.ID,
				Name = project.Name,
				Quantity = project.OverallQuantity,
			});
			return projectsShort;
		}

		private class ProjectShort
		{
			public long Id { get; set; }
			public string Name { get; set; }
			public int Quantity { get; set; }

		}
	}
}
