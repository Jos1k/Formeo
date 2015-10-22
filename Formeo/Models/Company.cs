using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class Company
	{
		public Company()
		{
			Users = new List<ApplicationUser>();
		}

		[Key]
		public long ID { get; set; }

		public string Name { get; set; }

		public string OrgNumber { get; set; }

		public string Country { get; set; }

		public string TaxNumber { get; set; }

		public bool IsCustomer { get; set; }

		public bool IsDeleted { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }

	}
}
