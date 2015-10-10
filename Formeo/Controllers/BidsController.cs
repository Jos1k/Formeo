using Formeo.BussinessLayer;
using Formeo.Controllers.CustomAttributes;
using Formeo.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace Formeo.Controllers
{
	public class BidsController : Controller
	{
		IPrintObjectsService _printObjectsService;
		public BidsController(IPrintObjectsService printObjectsService)
		{
			_printObjectsService = printObjectsService;
		}

		[HttpGet]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult GetBidModal(long printObjectId)
		{
			var printObject = _printObjectsService.GetPrintObjectsByIdJSON(printObjectId);

			_BidPrintObjectPartialViewModel viewModel = new _BidPrintObjectPartialViewModel();
			viewModel.PrintObjectJSON = printObject;

			return PartialView("_BidPrintObjectPartial", viewModel);
		}

		[HttpPost]
		public ActionResult CreateBid() 
		{
			throw new NotImplementedException();
		}
	}
}