using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class Order
	{
		public long ID { get; set; }
		public string  Name { get; set; }

		public long CustomerID { get; set; }

		public int StatusID { get; set; }

		public User Customer { get; set; }

		public User Producer { get; set; }

		public Status Status { get; set; }

		public ICollection<OrderLine> OrderLines { get; set; }
	}
}
