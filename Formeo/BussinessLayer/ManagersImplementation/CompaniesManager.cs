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

			return _dbContext.Companies
				.SingleOrDefault( company => company.ID == user.Company.ID );
		}
		public Company GetCompanyById( long companyId ) {
			return _dbContext
				.Companies.SingleOrDefault( company => company.ID == companyId );
		}

		public Company GetCurrentCompany() {
			ApplicationUser currentUser = _userManager.GetCurrentUser();
			return currentUser.Company;
		}

		public IEnumerable<Company> GetCompanies() {
			return _dbContext.Companies.Where( x => !x.IsDeleted ).ToArray();
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


		public ICollection<string> RemoveCompany( long companyId ) {
			Company company = _dbContext.Companies.Find( companyId );
			List<string> deletedUsers = null;
			if( company != null ) {
				company.IsDeleted = true;
				List<string> usersToDelete = company.Users.Select(x => x.Id).ToList();
				foreach (var userTodeleteId in usersToDelete)
				{
					ApplicationUser userToDelete = _userManager.GetUserById( userTodeleteId );
					userToDelete.IsDeleted = true;
					_dbContext.SaveChanges();
					_dbContext.Entry( userToDelete ).Reload();
				}
				_dbContext.SaveChanges();
				deletedUsers = company.Users.Select( x => x.Id ).ToList();
			} else {
				throw new InvalidOperationException( "Company not found!" );
			}
			return deletedUsers;
		}
		public void UpdateCompany( Company company ) {
			bool ifSuchCompanyPresent = _dbContext.Companies.Count( x => x.OrgNumber == company.OrgNumber ) > 1;
			if( !ifSuchCompanyPresent ) {
				Company resultCompany = _dbContext.Companies.Find( company.ID );
				resultCompany.Name = company.Name;
				resultCompany.OrgNumber = company.OrgNumber;
				resultCompany.TaxNumber = company.TaxNumber;
				resultCompany.Country = company.Country;
				_dbContext.SaveChanges();
			} else {
				throw new InvalidOperationException( "Company with such orgNumber already exist!" );
			}
		}

		#endregion
	}
}