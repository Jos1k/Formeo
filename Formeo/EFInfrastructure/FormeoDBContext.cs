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

		public FormeoDBContext()
			: base("FormeoDBContext")
		{
		}
		public DbSet<Company> Company { get; set; }
		public DbSet<Project> Project { get; set; }
		public DbSet<OrderLine> OrderLine { get; set; }
		public DbSet<PrintMaterial> PrintMaterial { get; set; }
		public DbSet<PrintObject> PrintObject { get; set; }
		public DbSet<Status> Status { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Bid> Bid { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			

			//modelBuilder.Entity<Project>()
			//	.HasOptional(p => p.Bids);

			//modelBuilder.Entity<Project>()
			//	.HasOptional(p => p.Bid)
			//	.WithMany()
			//	.HasForeignKey(x => x.ID);

			//modelBuilder.Entity<Bid>()
			//	.HasRequired(b => b.Project)
			//	.WithMany(p => p.Bids)
			//	.HasForeignKey(b => b.ID);

			//modelBuilder.Entity<OrderLine>()
			//	.HasRequired(ol => ol.Project)
			//	.WithMany(p => p.OrderLines)
			//	.HasForeignKey(m => m.Project);

			


			base.OnModelCreating(modelBuilder);
		}
	}
}