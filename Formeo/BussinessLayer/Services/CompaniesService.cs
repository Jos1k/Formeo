using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Services {
	public class CompaniesService : ICompaniesService {
		ICompaniesManager _manager;
		public CompaniesService( ICompaniesManager manager ) {
			_manager = manager;
		}

		public string GetCompaniesJSON() {
			var companies = _manager.GetCompanies();
			var short_companies = companies.Select(
				company => new {
					id = company.ID,
					orgNumber = company.OrgNumber,
					country = company.Country,
					taxNumber = company.TaxNumber,
					companyName = company.Name,
					isCustomer = company.IsCustomer
				} );
			return JsonConvert.SerializeObject( short_companies );
		}
		public string GetCompanyJSON(long sompanyId) {
			var company = _manager.GetCompanyById( sompanyId );
			var short_company = new {
				id = company.ID,
				orgNumber = company.OrgNumber,
				country = company.Country,
				taxNumber = company.TaxNumber,
				companyName = company.Name,
				isCustomer = company.IsCustomer
			};
			return JsonConvert.SerializeObject( short_company );
		}
		
	}
}
