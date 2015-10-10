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
		ICompaniesManager _companiesManager;
		ApplicationDbContext _dbcontext;

		public ProjectsManager(IPrintObjectsManager printObjectsManager,
			IUserManager userManager,
			ApplicationDbContext dbContext,
			ICompaniesManager companiesManager)
		{
			_printObjectsManager = printObjectsManager;
			_userManager = userManager;
			_dbcontext = dbContext;
			_companiesManager = companiesManager;
		}

		public Project CreateProject(
			string projectName,
			string creatorUserId,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo)
		{
			ApplicationUser creatorUser = _userManager.GetUserById(creatorUserId);

			if (creatorUser == null
				|| !_userManager.UserIsInRole(
				creatorUserId,
				StaticData.RoleNames.Customer))
			{
				return null;
			}

			int overallQuantity = 0;

			//need status manager...later
			OrderStatus newStatus =
				_dbcontext
					.Statuses
					.Where(stat => stat.CurrentOrderStatus == Formeo.Models.StaticData.OrderStatusEnum.InProgress)
					.Single();

			Company company = _companiesManager.GetCompanyByUserId(creatorUserId);

			Project newProject = new Project();
			newProject.Name = projectName;
			newProject.Address = deliveryInfo.Address;
			newProject.City = deliveryInfo.City;
			newProject.Country = deliveryInfo.Country;
			newProject.Surname = deliveryInfo.Surname;
			newProject.LastName = deliveryInfo.LastName;
			newProject.ZipCode = deliveryInfo.PostCode;
			newProject.Creator = creatorUser;
			newProject.Status = newStatus;

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
					ProjectInfo rel = new ProjectInfo();
					rel.PrintObject = poEntity;
					rel.Project = newProject;
					rel.Quantity = poInfo.Quantity;
					_dbcontext.ProjectsInfo.Add(rel);
					_dbcontext.SaveChanges();
				}
				overallQuantity += poInfo.Quantity;
			}

			_dbcontext.SaveChanges();
			return newProject;
		}

		public IEnumerable<Project> GetNewProjects()
		{
			throw new NotImplementedException();
		}


		public IEnumerable<Project> GetProjectsByStatus(StaticData.OrderStatusEnum ordersStatus)
		{
			return _dbcontext
			.Projects
			.Where(project => project.Status.CurrentOrderStatus == ordersStatus)
			.ToList();
		}

		public IEnumerable<Project> GetProjectByCreator(string customerId, StaticData.OrderStatusEnum orderStatus)
		{
			return _dbcontext
					.Projects
					.Where(project => project.Creator.Id == customerId
						&& project.Status.CurrentOrderStatus == orderStatus)
					.ToList();
		}

		public IEnumerable<Project> GetProjectsByCompany(long companyId, StaticData.OrderStatusEnum orderStatus)
		{
			var projects =
				from project in _dbcontext.Projects
				join projectInfo in _dbcontext.ProjectsInfo
				on project.ID equals projectInfo.ProjectId
				select project;
			return projects;
		}

	}
}