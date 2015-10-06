using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	public class ProjectsManager : IProjectsManager
	{
		private IPrintObjectsManager _printObjectsManager;
		IUserManager _userManager;
		ApplicationDbContext _dbcontext;

		public ProjectsManager(IPrintObjectsManager printObjectsManager, IUserManager userManager, ApplicationDbContext dbContext)
		{
			_printObjectsManager = printObjectsManager;
			_userManager = userManager;
			_dbcontext = dbContext;
		}

		public Project CreateProject(
			string projectName,
			int articleNo,
			string userId,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo)
		{
			ApplicationUser creatorUser = _userManager.GetCurrentUser();

			if (creatorUser == null 
				||!_userManager.UserIsInRole(
				userId, 
				StaticData.RoleNames.Customer))
			{
				return null;
			}

			int overallQuantity = 0;



			Project newProject = new Project();
			newProject.Name = projectName;
			newProject.ArticleNo = articleNo;
			newProject.Address = deliveryInfo.Address;
			newProject.City = deliveryInfo.City;
			newProject.Country = deliveryInfo.Country;
			newProject.Surname = deliveryInfo.Surname;
			newProject.LastName = deliveryInfo.LastName;
			newProject.ZipCode = deliveryInfo.PostCode;
			newProject.Customer = creatorUser;
			newProject.IsCompleted = false;

			_dbcontext.Projects.Add(newProject);
			//_dbcontext.SaveChanges();


			foreach (LayOrderPrintObjectInfo poInfo in printObjectInfo)
			{
				PrintObject poEntity = _dbcontext
					.PrintObjects
					.Where(po => po.ID == poInfo.PrintObjectId)
					.FirstOrDefault(); //the same hack
				//_printObjectsManager.GetPrintObjectById(poInfo.PrintObjectId);

				if (poEntity != null)
				{
					ProjectPrintObjectQuantityRelation rel = new ProjectPrintObjectQuantityRelation();
					rel.PrintObject = poEntity;
					rel.Project = newProject;
					rel.Quantity = poInfo.Quantity;
					_dbcontext.ProjectPrintObjectQuantityRelations.Add(rel);
					_dbcontext.SaveChanges();
				}
				overallQuantity += poInfo.Quantity;
			}

			newProject.OverallQuantity = overallQuantity;
			_dbcontext.SaveChanges();
			return newProject;
		}

		public IEnumerable<Project> GetProjectsByUserId(string userId,bool isCompleted)
		{
			var projects = GetAllProjectsByUserId(userId);
			return projects.Where(project => project.IsCompleted == isCompleted);
		}

		public IEnumerable<Project> GetAllProjectsByUserId(string userId)
		{
			return _dbcontext
				.Projects
				.Where(project => project.Customer.Id == userId)
				.ToList();
		}
	}
}