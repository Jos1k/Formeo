using Formeo.Controllers.CustomAttributes;
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


		[HttpGet]
		public ActionResult LayOrder(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = GetLayOrderViewModel(selectedPrintObjectIds);

			return PartialView("_LayOrderPartial", viewModel);
		}
		
		[HttpGet]
		public ActionResult LayOrderConfirm()
		{
			return PartialView("_LayOrderConfirmPartial");
		}

	
		[HttpPost]
		[JsonQueryParamFilter(Param = "printObjectInfo", JsonDataType = typeof(List<LayOrderPrintObjectInfo>))]
		[JsonQueryParamFilter(Param = "deliveryInfo", JsonDataType = typeof(DeliveryInfo))]
		public JsonResult CreateOrder(List<LayOrderPrintObjectInfo> printObjectInfo, DeliveryInfo deliveryInfo) 
		{
			CreateOrderHelp(printObjectInfo, deliveryInfo);

			return Json("");
		}

		#region Helpers
		//All code from helpers should be moved to managers
		//F**k it's mess here... should move this mess to injectable managers...later.

		private LayOrderViewModel GetLayOrderViewModel(long[] selectedPrintObjectIds)
		{
			LayOrderViewModel viewModel = new LayOrderViewModel();

			List<PrintObject> printObjects = DbContext.PrintObjects.Where(
					po => selectedPrintObjectIds.Contains(po.ID)
				).ToList();

			List<object> tempList = new List<object>();
			foreach (PrintObject printObject in printObjects)
			{
				tempList.Add(
					new {
						PrintObjectId = printObject.ID,
						ArtNo = printObject.ArticleNo,
						Name = printObject.Name,
						Quantity = 1
						});
			}

			viewModel.PrintObjectsInfoJSON = JsonConvert.SerializeObject(tempList);

			return viewModel;
		}

		private void CreateOrderHelp(List<LayOrderPrintObjectInfo> printObjectInfo, DeliveryInfo deliveryInfo) 
		{
			var currentContext = DbContext;
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
			}
		}

		#endregion
	}
}