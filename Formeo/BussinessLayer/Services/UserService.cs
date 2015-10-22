using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Services {
	public class UserService : IUserService {
		IUserManager _manager;

		public UserService( IUserManager manager ) {
			_manager = manager;
		}
		public string GetUsersByRoleJSON( string roleName ) {
			var users = _manager.GetUsersByRole( roleName );

			var users_short = users.Select
				( user => new {
					Id = user.Id,
					UserName = user.UserName,
					Company = user.Company == null ? "<No Data>" : user.Company.Name,
					Email = string.IsNullOrWhiteSpace( user.Email ) ? "<No Data>" : user.Email,
					Address = string.IsNullOrWhiteSpace( user.Adress ) ? "<No Data>" : user.Adress,
					Postal = string.IsNullOrWhiteSpace( user.ZipCode ) ? "<No Data>" : user.ZipCode,
					City = string.IsNullOrWhiteSpace( user.City ) ? "<No Data>" : user.City,
					Country = string.IsNullOrWhiteSpace( user.Country ) ? "<No Data>" : user.Country,
					//IsProducer = roleName == StaticData.RoleNames.Producer,
					//IsCustomer = roleName == StaticData.RoleNames.Customer,
					//IsAdmin = roleName == StaticData.RoleNames.Admin
					SelectedRole = _manager.GetRoleNameByRoleId( user.Roles.FirstOrDefault().RoleId )
				}
				)
				.ToArray();
			return JsonConvert.SerializeObject( users_short );
		}

		#region IUserService Members


		public string GetUsersByIdJSON( string id ) {
			var user = _manager.GetUserById( id );
			string roleName = "";

			var users_short = new {
				Id = user.Id,
				UserName = user.UserName,
				Company = user.Company == null ? "<No Data>" : user.Company.Name,
				Email = string.IsNullOrWhiteSpace( user.Email ) ? "<No Data>" : user.Email,
				Address = string.IsNullOrWhiteSpace( user.Adress ) ? "<No Data>" : user.Adress,
				Postal = string.IsNullOrWhiteSpace( user.ZipCode ) ? "<No Data>" : user.ZipCode,
				City = string.IsNullOrWhiteSpace( user.City ) ? "<No Data>" : user.City,
				Country = string.IsNullOrWhiteSpace( user.Country ) ? "<No Data>" : user.Country,
				SelectedRole = _manager.GetRoleNameByRoleId( user.Roles.FirstOrDefault().RoleId )

			};
			return JsonConvert.SerializeObject( users_short );
		#endregion
		}
	}
}
