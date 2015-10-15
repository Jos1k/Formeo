using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.Models.HelperModels
{
	public class PrintObjectFileInfo
	{
		public string ArtNo { get; set; }
		public string ProductName { get; set; }

		public string PrintMaterial { get; set; }

		public HttpPostedFileBase File { get; set; }

		public PrintObjectFileInfo(string artNo, string productName, HttpPostedFileBase file)
		{
			ArtNo = artNo;
			ProductName = productName;
			File = file;
		}
	}
}