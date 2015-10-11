using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	class CompaniesManager : ICompaniesManager
	{
		ApplicationDbContext _dbContext;
		IUserManager _userManager;
		public CompaniesManager(ApplicationDbContext context, IUserManager userManager)
		{
			_dbContext = context;
			_userManager = userManager;
		}
		public Company GetCompanyByUserId(string userId)
		{
			var user = _userManager.GetUserById(userId);

			if (user == null)
			{
				return null;
			}

			return _dbContext
					.Companies
					.Where(company => company.ID == user.Company.ID)
					.SingleOrDefault();
		}
		public Company GetCompanyById(long companyId)
		{
			return _dbContext
				.Companies
				.Where(company => company.ID == companyId)
				.SingleOrDefault();
		}
	}
}