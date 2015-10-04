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

		[InjectionConstructor]
		public ProjectController(
			IPrintObjectService printObjectService,
			IUserManager userManager)
		{
			_printObjectService = printObjectService;
			_userManager = userManager;
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
			viewModel.PrintObjectsInfoJSON = _printObjectService.GetExclusivePrintObjectsByIdsForUserJSON(currentUser, selectedPrintObjectIds);


			return PartialView("_AddProductsPartial", viewModel);
		}


		[HttpPost]
		[JsonQueryParamFilter(Param = "printObjectInfo", JsonDataType = typeof(List<LayOrderPrintObjectInfo>))]
		[JsonQueryParamFilter(Param = "deliveryInfo", JsonDataType = typeof(DeliveryInfo))]
		public JsonResult CreateOrder(List<LayOrderPrintObjectInfo> printObjectInfo, DeliveryInfo deliveryInfo)
		{
			int overallQuantity;
			Project newProject = CreateOrder_Helper(printObjectInfo, deliveryInfo, out overallQuantity);
			var result = JsonConvert.SerializeObject(
				new
				{
					OrderId = newProject.ID,
					Quantity = overallQuantity
				});
			return Json(result);
		}

		#region Helpers
		//All code from helpers should be moved to managers
		//F**k it's mess here... should move this mess to injectable managers...later.

		private LayOrderViewModel GetLayOrderViewModel(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			viewModel.PrintObjectsInfoJSON =
				_printObjectService.GetPrintObjectsByIdsJSON(selectedPrintObjectIds);

			return viewModel;
		}

		private Project CreateOrder_Helper(List<LayOrderPrintObjectInfo> printObjectInfo, DeliveryInfo deliveryInfo, out int overallQuantity)
		{
			var currentContext = DbContext;
			overallQuantity = 0;

			Project newProject = new Project();
			newProject.Address = deliveryInfo.Address;
			newProject.City = deliveryInfo.City;
			newProject.Country = deliveryInfo.Country;
			newProject.Surname = deliveryInfo.Surname;
			newProject.LastName = deliveryInfo.LastName;
			newProject.ZipCode = deliveryInfo.PostCode;

			currentContext.Projects.Add(newProject);
			currentContext.SaveChanges();

			List<PrintObject> projects = new List<PrintObject>(); ;

			foreach (LayOrderPrintObjectInfo poInfo in printObjectInfo)
			{
				PrintObject poEntity = currentContext.PrintObjects
					.Where(po => po.ID == poInfo.PrintObjectId)
					.FirstOrDefault();

				if (poEntity != null)
				{
					ProjectPrintObjectQuantityRelation rel = new ProjectPrintObjectQuantityRelation();
					rel.PrintObject = poEntity;
					rel.Project = newProject;
					rel.Quantity = poInfo.Quantity;
					currentContext.ProjectPrintObjectQuantityRelations.Add(rel);
					currentContext.SaveChanges();
				}
				overallQuantity += poInfo.Quantity;
			}

			return newProject;
		}

		//this method should extract as extension


		#endregion
	}
}