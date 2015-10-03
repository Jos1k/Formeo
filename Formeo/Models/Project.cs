using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class Project
	{
		public Project()
		{
			//PrintObjects = new List<PrintObject>();
			Bids = new List<Bid>();
		}
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }

		public virtual ApplicationUser Customer { get; set; }

		public virtual ApplicationUser Producer { get; set; }

		public virtual Status Status { get; set; }

		public virtual Bid WinningBid { get; set; }

		public virtual ICollection<Bid> Bids { get; set; }

		public virtual ICollection<ProjectPrintObjectQuantityRelation> ProjectPrintObjectQuantityRelations { get; set; }


		#region DeliveryInfo

		public string Surname { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string ZipCode { get; set; }

		public string City { get; set; }
		public string Country { get; set; }

		#endregion
	}
}
