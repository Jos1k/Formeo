using Formeo.BussinessLayer.Interfaces;
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
		public ProjectService(IUserManager userManager, IProjectsManager projectsManager)
		{
			_userManager = userManager;
			_projectsManager = projectsManager;
		}
		public string GetProjectsByUserJSON(string userId, bool isActive)
		{
			var projects = _projectsManager.GetProjectsByUserId(userId,isActive);

			var projectsShort = projects.Select
				(project =>
					new
					{
						ProjectId = project.ID,
						Name = project.Name,
						Quantity = project.OverallQuantity,
						IsCompleted = project.IsCompleted,
						ArticleNo = project.ArticleNo
					});
			return JsonConvert.SerializeObject(projectsShort);
		}


	}
}
