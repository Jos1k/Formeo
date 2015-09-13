using Formeo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Formeo.EFInfrastructure
{
	public class FormeoDBContext : DbContext
	{

		public FormeoDBContext()
			: base("DefaultConnection")
		{
		}
		public DbSet<Company> Company { get; set; }
		public DbSet<Project> Project { get; set; }
		public DbSet<OrderLine> OrderLine { get; set; }
		public DbSet<PrintMaterial> PrintMaterial { get; set; }
		public DbSet<PrintObject> PrintObject { get; set; }
		public DbSet<Status> Status { get; set; }
		public DbSet<ApplicationUser> User { get; set; }
		public DbSet<Bid> Bid { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
			modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
			modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

			base.OnModelCreating(modelBuilder);
		}
	}
}