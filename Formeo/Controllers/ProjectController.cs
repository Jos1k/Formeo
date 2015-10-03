using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Formeo.Controllers
{
	public class ProjectController : Controller
	{

		ApplicationDbContext DbContext { get { return new ApplicationDbContext(); } }
		// GET: Project
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult LayOrder(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = GetLayOrderViewModel(selectedPrintObjectIds);

			return PartialView("_LayOrderPartial", viewModel);
		}


		#region Helpers

		private LayOrderViewModel GetLayOrderViewModel(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			List<PrintObject> printObjects = DbContext.PrintObjects.Where(
					po => selectedPrintObjectIds.Contains(po.ID)
				).ToList();

			List<LayOrderPrintObjectInfo> tempList = new List<LayOrderPrintObjectInfo>();
			foreach (PrintObject printObject in printObjects)
			{
				tempList.Add(
					new LayOrderPrintObjectInfo(
						printObject.ID,
						printObject.ArticleNo,
						printObject.Name,
						1));
			}

			viewModel.PrintObjectsInfoJSON = JsonConvert.SerializeObject(tempList);

			return viewModel;
		}

		#endregion
	}
}