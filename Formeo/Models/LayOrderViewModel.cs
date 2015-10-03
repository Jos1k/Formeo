using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	//part of project
	public class LayOrderViewModel
	{

		public LayOrderViewModel()
		{
			//PrintObjectsInfo = new List<LayOrderPrintObjectInfo>();
		}
		//public ICollection<LayOrderPrintObjectInfo> PrintObjectsInfo { get; set; }

		public string PrintObjectsInfoJSON { get; set; }
		public string SurName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string ZipCode { get; set; }

		public string City { get; set; }
		public string Country { get; set; }

	}

	public class LayOrderPrintObjectInfo
	{
		public LayOrderPrintObjectInfo(long id, long artNo, string name, int quantity)
		{
			PrintObjectId = id;
			ArtNo = artNo;
			Name = name;
			Quantity = quantity;
		}
		public long PrintObjectId { get; set; }
		public long ArtNo { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
	}

}