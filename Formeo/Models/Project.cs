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
			PrintObjects = new List<PrintObject>();
			Bids = new List<Bid>();
		}
		[Key]
		public long ID { get; set; }
		public string  Name { get; set; }

		public virtual ApplicationUser Customer { get; set; }

		public virtual ApplicationUser Producer { get; set; }

		public virtual Status Status { get; set; }

		public virtual Bid WinningBid { get; set; }

		public virtual ICollection<Bid> Bids { get; set; }

		public virtual ICollection<PrintObject> PrintObjects { get; set; }
	}
}
