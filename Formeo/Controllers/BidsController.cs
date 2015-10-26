using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Controllers.CustomAttributes;
using Formeo.Models;
using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Formeo.Controllers
{
	public class BidsController : Controller
	{
		IPrintObjectsService _printObjectsService;
		IBidsService _bidsService;
		public BidsController(IPrintObjectsService printObjectsService, IBidsService bidsService)
		{
			_printObjectsService = printObjectsService;
			_bidsService = bidsService;
		}


		/// <summary>
		/// for now - used for producer when he tries to bid product
		/// </summary>
		/// <param name="printObjectId"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetBidModal()
		{
			return PartialView("_BidPrintObjectPartial");
		}

		[HttpPost]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult GetBidPrintObject(long printObjectId)
		{
			string result = _printObjectsService.GetPrintObjectsByIdForProducerJSON(printObjectId);
			return Json(result);
		}


		[HttpPost]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		[JsonQueryParamFilter(JsonDataType = typeof(decimal), Param = "price")]
		public ActionResult CreateBid(long printObjectId, decimal price, string currency)
		{
			return Json(_bidsService.CreateBidJSON(printObjectId, price, currency));
		}

		[HttpGet]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult GetBidsForPrintObject(long printObjectId)
		{
			_BidsForPrintObjectsViewModel viewModel = new _BidsForPrintObjectsViewModel();
			viewModel.BidsJSON = _bidsService.GetBidsForPrintObjectJSON(printObjectId);

			return PartialView("_BidsForPrintObjectPatrial", viewModel);
		}

	}
}