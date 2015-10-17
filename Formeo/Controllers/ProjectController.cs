using Formeo.BussinessLayer;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Controllers.CustomAttributes;
using Formeo.Models;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Formeo.Controllers
{
	public class ProjectController : Controller
	{

		ApplicationDbContext DbContext { get { return new ApplicationDbContext(); } }

		IPrintObjectsService _printObjectService;
		IUserManager _userManager;
		IProjectsManager _projectManager;
		ICompaniesManager _companiesManager;

		[InjectionConstructor]
		public ProjectController
		(
			IPrintObjectsService printObjectService,
			IUserManager userManager,
			IProjectsManager projectManager,
			ICompaniesManager companiesManager
		)
		{
			_printObjectService = printObjectService;
			_userManager = userManager;
			_projectManager = projectManager;
			_companiesManager = companiesManager;
		}

		[HttpGet]
		public ActionResult LayOrder(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			viewModel.PrintObjectsInfoJSON =
				_printObjectService
				.GetPrintObjectsByIdsForCustomerJSON(selectedPrintObjectIds);

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

			AddProductsViewModel viewModel = new AddProductsViewModel();
			var currentUser = _userManager.GetCurrentUser();
			Company company = _companiesManager.GetCompanyByUserId(currentUser.Id);
			if (selectedPrintObjectIds == null || selectedPrintObjectIds.Count() == 0)
			{
				viewModel.PrintObjectsInfoJSON =
					_printObjectService.GetPrintObjectsByCompanyCreatorJSON(company.ID);
			}
			else
			{
				viewModel.PrintObjectsInfoJSON =
					_printObjectService
					.GetExclusivePrintObjectsByIdsForCompanyJSON(
						company.ID,
						selectedPrintObjectIds
					);
			}

			return PartialView("_AddProductsPartial", viewModel);
		}

		[HttpPost]
		[JsonQueryParamFilter(Param = "printObjectInfo", JsonDataType = typeof(List<LayOrderPrintObjectInfo>))]
		[JsonQueryParamFilter(Param = "deliveryInfo", JsonDataType = typeof(DeliveryInfo))]
		public ActionResult CreateOrder(
			string orderName,
			List<LayOrderPrintObjectInfo> printObjectInfo,
			DeliveryInfo deliveryInfo)
		{

			ApplicationUser currentUser = _userManager.GetCurrentUser();

			Project newProject = _projectManager.CreateProject(
				orderName,
				currentUser.Id,
				printObjectInfo,
				deliveryInfo);

			var result = new
			{
				ProjectId = newProject.ID,
				Name = newProject.Name,
				Quantity = newProject.OverallQuantity,
				IsCompleted = false, //hack. should fix later to do by services
			};

			return Json(result);
		}

		[HttpPost]
		[JsonQueryParamFilter(Param = "projectId", JsonDataType = typeof(long))]
		[JsonQueryParamFilter(Param = "printObjectId", JsonDataType = typeof(long))]
		[JsonQueryParamFilter(Param = "status", JsonDataType = typeof(Formeo.Models.StaticData.PrintObjectStatusEnum))]
		public ActionResult SetProductState(long projectId, long printObjectId, Formeo.Models.StaticData.PrintObjectStatusEnum status)
		{
			_projectManager.SetPrintObjectStatus(projectId, printObjectId, status);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}
	}
}