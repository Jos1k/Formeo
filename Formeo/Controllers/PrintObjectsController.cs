using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Controllers.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Formeo.Controllers
{
	public class PrintObjectsController : Controller
	{
		IPrintObjectsManager _printObjecsManager;

		public PrintObjectsController(IPrintObjectsManager printObjecsManager)
		{
			_printObjecsManager = printObjecsManager;
		}
		[HttpPost]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult ToggleIsNeedPrintObjectBid(long printObjectId)
		{
			bool toggleRes = _printObjecsManager.ToggleIsNeedBid(printObjectId);
			Console.WriteLine(printObjectId);
			return Json(toggleRes);
		}
	}
}