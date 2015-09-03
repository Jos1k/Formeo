using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class Status
	{
		public long ID { get; set; }
		public string Name { get; set; }

		public ICollection<Order> Orders { get; set; }
	}
}
