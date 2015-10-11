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
		[Required]
		public decimal Price { get; set; }
		[Required]
		public virtual Company CompanyProducer { get; set; }
		[Required]
		public virtual PrintObject PrintObject { get; set; }
	}
}
