using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class OrderStatus
	{
		[Key]
		public long ID { get; set; }
		public  Formeo.Models.StaticData.OrderStatusEnum CurrentOrderStatus { get; set; }

		public virtual ICollection<Project> Projects { get; set; }
	}
}
