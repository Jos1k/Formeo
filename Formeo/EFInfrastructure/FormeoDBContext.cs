using Formeo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Formeo.EFInfrastructure
{
	public class FormeoDBContext : DbContext
	{
		public DbSet<Company> Company { get; set; }
		public DbSet<Order> Order { get; set; }
		public DbSet<OrderLine> OrderLine { get; set; }
		public DbSet<PrintMaterial> PrintMaterial { get; set; }
		public DbSet<PrintObject> PrintObject { get; set; }
		public DbSet<Status> Status { get; set; }
		public DbSet<User> User { get; set; }
	}
}