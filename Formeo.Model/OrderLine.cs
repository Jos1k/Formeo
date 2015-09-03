using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class OrderLine
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public long OrderID { get; set; }

		public Order Order { get; set; }
		public PrintObject PrintObjects { get; set; }
	}
}
