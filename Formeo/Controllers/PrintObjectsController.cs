using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Controllers.CustomAttributes;
using Formeo.Models;
using Formeo.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Formeo.Controllers
{
	public class PrintObjectsController : Controller
	{
		IPrintObjectsManager _printObjecsManager;
		IPrintObjectsService _printObjectsService;
		public PrintObjectsController(IPrintObjectsManager printObjecsManager, IPrintObjectsService printObjectsService)
		{
			_printObjecsManager = printObjecsManager;
			_printObjectsService = printObjectsService;
		}
		[HttpPost]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult ToggleIsNeedPrintObjectBid(long printObjectId)
		{
			bool toggleRes = _printObjecsManager.ToggleIsNeedBid(printObjectId);
			Console.WriteLine(printObjectId);
			return Json(toggleRes);
		}

		[HttpPost]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "producerCompanyId")]
		[JsonQueryParamFilter(JsonDataType = typeof(long), Param = "printObjectId")]
		public ActionResult AssingProducerToPrintObject(long producerCompanyId, long printObjectId)
		{
			string refreshedPrintObject = _printObjectsService.AssignProducerToPrintObject(producerCompanyId, printObjectId);
			return Json(refreshedPrintObject);
		}

		public sealed class Product
		{
			public string artNo { get; set; }
			public string productName { get; set; }
		}

		[HttpPost]
		public ActionResult UploadProducts(IEnumerable<Product> products, IEnumerable<HttpPostedFileBase> files)
		{

			IEnumerable<PrintObjectFileInfo> res = products.Zip(files,
				(product, file) =>
					new PrintObjectFileInfo(product.artNo, product.productName, file)
				).ToArray();

			return Json(_printObjectsService.UploadProducts(res));
		}

		//[HttpGet]
		public ActionResult UploadProductShowModal()
		{
			return PartialView("_UploadProductModal");
		}

		[HttpPost]
		public FileResult Download( long printObjectId ) {
			return new FileStreamResult( new FileStream( _printObjecsManager.GetPrintObjectFilePath( printObjectId ), FileMode.Open ), "application/pdf" ); 
				//File( _printObjecsManager.GetPrintObjectFilePath( printObjectId ), System.Net.Mime.MediaTypeNames.Application.Octet );
		}
	}
}