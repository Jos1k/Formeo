using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class Status
	{
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Project> Projects { get; set; }
	}
}
