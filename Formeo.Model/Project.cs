using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class Project
	{
		[Key]
		public long ID { get; set; }
		public string  Name { get; set; }

		public virtual User Customer { get; set; }

		public virtual User Producer { get; set; }

		public virtual Status Status { get; set; }

		public virtual Bid Bid { get; set; }

		public virtual ICollection<Bid> Bids { get; set; }

		public virtual ICollection<OrderLine> OrderLines { get; set; }
	}
}
