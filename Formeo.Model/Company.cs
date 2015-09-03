using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class Company
	{
		public long ID { get; set; }

		public string Name { get; set; }

		public string OrgNumber { get; set; }

		public string Country { get; set; }

		public string TaxNumber { get; set; }

		public ICollection<User> Customers { get; set; }

	}
}
