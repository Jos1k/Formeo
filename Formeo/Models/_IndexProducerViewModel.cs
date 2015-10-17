using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	public class _IndexProducerViewModel
	{
		public string Dashboard_BidRequestedPrintObjects { get; set; }
		public string Dashboard_PrintObjectsDelivered { get; set; }
		public string Storage_PrintObjects { get; set; }
		public string Orders_OrdersInQueue{ get; set; }
		public string Orders_OrdersInProduction { get; set; }
	}
}