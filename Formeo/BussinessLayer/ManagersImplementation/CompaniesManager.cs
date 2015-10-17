using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation {
	class CompaniesManager : ICompaniesManager {
		ApplicationDbContext _dbContext;
		IUserManager _userManager;
		public CompaniesManager( ApplicationDbContext context, IUserManager userManager ) {
			_dbContext = context;
			_userManager = userManager;
		}
		public Company GetCompanyByUserId( string userId ) {
			var user = _userManager.GetUserById( userId );

			if( user == null ) {
				return null;
			}

			return _dbContext
					.Companies
					.Where( company => company.ID == user.Company.ID )
					.SingleOrDefault();
		}
		public Company GetCompanyById( long companyId ) {
			return _dbContext
				.Companies
				.Where( company => company.ID == companyId )
				.SingleOrDefault();
		}

		public Company GetCurrentCompany() {
			ApplicationUser currentUser = _userManager.GetCurrentUser();
			return currentUser.Company;
		}

		public IEnumerable<Company> GetCompanies() {
			return _dbContext.Companies.ToArray();
		}

		#region ICompaniesManager Members


		public long CreateCompany( Company company ) {
			bool ifSuchCompanyPresent = _dbContext.Companies.Any( x => x.OrgNumber == company.OrgNumber );
			if( !ifSuchCompanyPresent ) {
				_dbContext.Companies.Add( company );
				_dbContext.SaveChanges();
			} else {
				throw new InvalidOperationException( "Company with such orgNumber already exist!" );
			}
			return company.ID;
		}

		#endregion

		#region ICompaniesManager Members


		public void RemoveCompany( long companyId ) {
			Company company = _dbContext.Companies.Find( companyId );
			if( company != null ) {
				_dbContext.Companies.Remove( company );
				_dbContext.SaveChanges();
			} else {
				throw new InvalidOperationException( "Company not found!" );
			}
		}

		#endregion
	}
}