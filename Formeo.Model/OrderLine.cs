using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class OrderLine
	{
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }
		public virtual Project Project { get; set; }
		public virtual PrintObject PrintObject { get; set; }
	}
}
