﻿using Formeo.BussinessLayer.Interfaces;
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
			newProject.CompanyCreator = company;

			_dbcontext.Projects.Add(newProject);
			_dbcontext.SaveChanges();

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
					projInfo.CompanyProducer = poEntity.CompanyProducer;

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

		public IEnumerable<Project> GetProjectByCreatorCompany(long companyId, StaticData.OrderStatusEnum orderStatus)
		{
			var customersProjects = _dbcontext
					.Projects
					.Include("CompanyCreator")
					.Where(project => project.CompanyCreator.ID == companyId)
					.ToList();
			return customersProjects.Where(project => project.Status == orderStatus); //hack. don't touch it
		}
	
		public IEnumerable<ProjectInfo> GetProjectInfosForProducer(long companyId, StaticData.PrintObjectStatusEnum printObjectStatus) 
		{
			return _dbcontext
				.ProjectInfos
				.Include("Project")
				.Include("PrintObject")
				.Where(
					projectInfo => projectInfo.CompanyProducer.ID == companyId
								&& projectInfo.Status == printObjectStatus
				);
		}

		public IEnumerable<ProjectInfo> GetProjectInfosByProjectId(long projectId) {
			return _dbcontext
				.ProjectInfos
				.Include("CompanyProducer")
				.Include("PrintObject")
				.Where(projectInfo => projectInfo.ProjectId == projectId);
		}

		public void SetPrintObjectStatus(long projectId, long printObjectId, StaticData.PrintObjectStatusEnum status)
		{
			ProjectInfo projectInfo = _dbcontext.ProjectInfos
				.Find(projectId, printObjectId);
			projectInfo.Status = status;
			_dbcontext.SaveChanges();
			_dbcontext.Entry(projectInfo).Reload();
		}

	}
}