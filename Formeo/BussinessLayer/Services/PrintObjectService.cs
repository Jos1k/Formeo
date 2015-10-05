using Formeo.BussinessLayer.Interfaces;
using Formeo.BussinessLayer.ManagersImplementation;
using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.Services
{
	public class PrintObjectService : IPrintObjectService
	{
		IPrintObjectsManager _printObjectsManager;
		public PrintObjectService(IPrintObjectsManager printObjectsManager)
		{
			_printObjectsManager = printObjectsManager;
		}


		#region IPrintObjectService Members
		public string GetPrintObjectsByIdsJSON(IEnumerable<long> printObjectIds)
		{
			if (printObjectIds == null || printObjectIds.Count() == 0)
			{
				return string.Empty;
			}

			var printObjects = _printObjectsManager.GetPrintObjectsByIds(printObjectIds);

			if (printObjects == null || printObjects.Count() == 0)
			{
				return string.Empty;
			}

			List<object> tempList = new List<object>();
			foreach (PrintObject printObject in printObjects)
			{
				tempList.Add(
					new
					{
						PrintObjectId = printObject.ID,
						ArtNo = printObject.ArticleNo,
						Name = printObject.Name,
						Quantity = 1
					});
			}
			return JsonConvert.SerializeObject(tempList);
		}
		public string GetPrintObjectsForUserJSON(string userId)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsForUser(userId);
			var printObjectsShort = printObjects.Select(
				 po => new
				 {
					 Id = po.ID,
					 ArtNo = po.ArticleNo,
					 Name = po.Name,
				 }
				).ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetExclusivePrintObjectsByIdsForUserJSON(string userId, IEnumerable<long> printObjectIdsToExlude)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsForUser(userId);
			var printObjectsShort = printObjects
				.Where(po => !printObjectIdsToExlude.Contains(po.ID))
				.Select(po =>
					new
					{
						Id = po.ID,
						ArtNo = po.ArticleNo,
						Name = po.Name
					})
				 .ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		#endregion

	}
}