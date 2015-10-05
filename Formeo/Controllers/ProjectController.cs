using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Controllers.CustomAttributes;
using Formeo.Models;
using Microsoft.Practices.Unity;
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

		IPrintObjectService _printObjectService;
		IUserManager _userManager;
		IProjectsManager _projectManager;

		[InjectionConstructor]
		public ProjectController(
			IPrintObjectService printObjectService,
			IUserManager userManager,
			IProjectsManager projectManager)
		{
			_printObjectService = printObjectService;
			_userManager = userManager;
			_projectManager = projectManager;
		}

		[HttpGet]
		public ActionResult LayOrder(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			viewModel.PrintObjectsInfoJSON =
				_printObjectService.GetPrintObjectsByIdsJSON(selectedPrintObjectIds);

			return PartialView("_LayOrderPartial", viewModel);
		}

		[HttpGet]
		public ActionResult LayOrderConfirm()
		{
			return PartialView("_LayOrderConfirmPartial");
		}

		[HttpGet]
		[JsonQueryParamFilter(Param = "selectedPrintObjectIds", JsonDataType = typeof(List<long>))]
		public ActionResult AddProducts(List<long> selectedPrintObjectIds)
		{

			if (selectedPrintObjectIds == null || selectedPrintObjectIds.Count() == 0)
			{
				return PartialView("_AddProductsPartial", new AddProductsViewModel());
			}

			AddProductsViewModel viewModel = new AddProductsViewModel();
			var currentUser = _userManager.GetCurrentUser();
			viewModel.PrintObjectsInfoJSON = _printObjectService.GetExclusivePrintObjectsByIdsForUserJSON(currentUser.Id, selectedPrintObjectIds);


			return PartialView("_AddProductsPartial", viewModel);
		}


		[HttpPost]
		[JsonQueryParamFilter(Param = "articleNo", JsonDataType = typeof(int))]
		[JsonQueryParamFilter(Param = "printObjectInfo", JsonDataType = typeof(List<LayOrderPrintObjectInfo>))]
		[JsonQueryParamFilter(Param = "deliveryInfo", JsonDataType = typeof(DeliveryInfo))]
		public ActionResult CreateOrder(
			string orderName,
			int articleNo,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo)
		{
			int overallQuantity;

			ApplicationUser currentUser = _userManager.GetCurrentUser();

			Project newProject = _projectManager.CreateProject(
				orderName,
				articleNo,
				currentUser.Id,
				printObjectInfo,
				deliveryInfo);

			var result = new {
					ProjectId = newProject.ID,
					Name = newProject.Name,
					Quantity = newProject.OverallQuantity,
					IsCompleted = newProject.IsCompleted,
					ArticleNo = newProject.ArticleNo
				};
			return Json(result);
		}

		#region Helpers

		private LayOrderViewModel GetLayOrderViewModel(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			viewModel.PrintObjectsInfoJSON =
				_printObjectService.GetPrintObjectsByIdsJSON(selectedPrintObjectIds);

			return viewModel;
		}

		#endregion
	}
}