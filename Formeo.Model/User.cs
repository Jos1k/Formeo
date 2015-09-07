using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class User
	{
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }

		public string Email { get; set; }
		public string Password { get; set; }
		public string Adress { get; set; }

		public string ZipCode { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public virtual Company Company { get; set; }

		public virtual ICollection<Project> Projects { get; set; }

		public virtual UserType UserType { get; set; }

	}
}
