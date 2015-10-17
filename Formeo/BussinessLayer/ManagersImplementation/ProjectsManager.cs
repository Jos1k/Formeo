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
		private IUserManager _userManager;
		private ICompaniesManager _companiesManager;
		private IBidsManager _bidsManager;
		private ApplicationDbContext _dbcontext;

		public ProjectsManager(IPrintObjectsManager printObjectsManager,
			IUserManager userManager,
			ApplicationDbContext dbContext,
			ICompaniesManager companiesManager,
			IBidsManager bidsManager)
		{
			_printObjectsManager = printObjectsManager;
			_userManager = userManager;
			_dbcontext = dbContext;
			_companiesManager = companiesManager;
			_bidsManager = bidsManager;
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
			newProject.Status = Formeo.Models.StaticData.OrderStatusEnum.InProgress;
			newProject.CompanyCreator = company;

			_dbcontext.Projects.Add(newProject);
			//_dbcontext.SaveChanges();

			decimal totalPrice = 0;
			foreach (LayOrderPrintObjectInfo poInfo in printObjectInfo)
			{
				PrintObject poEntity = _printObjectsManager.GetPrintObjectById(poInfo.PrintObjectId);
				Bid selectedBid = _bidsManager.GetSelecetdBidForPrintObject(poInfo.PrintObjectId);

				if (poEntity != null && selectedBid != null)
				{
					ProjectInfo projInfo = new ProjectInfo();
					projInfo.PrintObject = poEntity;
					projInfo.Project = newProject;
					projInfo.Quantity = poInfo.Quantity;
					projInfo.Status = Formeo.Models.StaticData.PrintObjectStatusEnum.InQueue;

					totalPrice += selectedBid.Price * poInfo.Quantity;

					_dbcontext.ProjectInfos.Add(projInfo);
					_dbcontext.SaveChanges();
				}
				overallQuantity += poInfo.Quantity;
			}
			newProject.OrderPrice = totalPrice;
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
			.Where(project => project.Status == ordersStatus)
			.ToList();
		}

		public IEnumerable<Project> GetProjectByCreator(string customerId, StaticData.OrderStatusEnum orderStatus)
		{
			return _dbcontext
					.Projects
					.Where(project => project.Creator.Id == customerId
						&& project.Status == orderStatus)
					.ToList();
		}
		public IEnumerable<ProjectInfo> GetProjectInfosForProducer(long companyId, StaticData.PrintObjectStatusEnum printObjectStatus) 
		{
			return
				from projectInfo in _dbcontext.ProjectInfos
												.Include("Project")
												.Include("PrintObject")
				join printObject in _dbcontext.PrintObjects on projectInfo.PrintObjectId equals printObject.ID
				where printObject.CompanyProducer.ID == companyId
					&& projectInfo.Status == printObjectStatus
				select projectInfo;
		}
		

	}
}