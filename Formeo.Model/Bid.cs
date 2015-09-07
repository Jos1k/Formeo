using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class Bid
	{
		[Key]
		[ForeignKey("Project")]
		public long ID { get; set; }
		public decimal Price { get; set; }

		public virtual User Producer { get; set; }

		public virtual Project Project { get; set; }
	}
}
