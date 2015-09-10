using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class Bid
	{
		[Key]
		public long ID { get; set; }
		public decimal Price { get; set; }
		[Required]
		public virtual ApplicationUser Producer { get; set; }
		[Required]
		public virtual Project Project { get; set; }
	}
}
