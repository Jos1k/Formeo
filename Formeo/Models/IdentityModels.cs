using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;

namespace Formeo.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{

		public string Adress { get; set; }

		public string ZipCode { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public virtual Company Company { get; set; }

		public virtual ICollection<Project> Projects { get; set; }

		public virtual ICollection<PrintObject> PrintObjects { get; set; }


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDisposable
	{
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{

		}

		public DbSet<Company> Companies { get; set; }
		public DbSet<Project> Projects { get; set; }
	//	public DbSet<PrintMaterial> PrintMaterials { get; set; }
		public DbSet<PrintObject> PrintObjects { get; set; }
		public DbSet<Bid> Bids { get; set; }

		public DbSet<ProjectInfo> ProjectInfos { get; set; } 

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}
