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
	public class PrintObjectService : IPrintObjectsService
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

			IReadOnlyList<PrintObjectShort> printObjectsShort = 
				printObjects.Select(po =>
					PrintObjectToShort(po))
					.ToArray();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetPrintObjectsByIdJSON(long printObjectId)
		{
			var printObject = _printObjectsManager.GetPrintObjectsById(printObjectId);

			if (printObject == null )
			{
				return string.Empty;
			}

			PrintObjectShort printObjectShort = PrintObjectToShort(printObject);

			return JsonConvert.SerializeObject(printObjectShort);
		}

		public string GetPrintObjectsByCompanyCreatorJSON(long companyId)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCreatorCompany(companyId);
			var printObjectsShort = printObjects.Select(
				 po => PrintObjectToShort(po)
				).ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetExclusivePrintObjectsByIdsForCompanyJSON(long companyId, IEnumerable<long> printObjectIdsToExlude)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCreatorCompany(companyId);

			var printObjectsShort = printObjects
				.Where(po => !printObjectIdsToExlude.Contains(po.ID))
				.Select(po =>
					PrintObjectToShort(po)
					)
				 .ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetNeedBidPrintObjectsForProducerJSON(string producerId,bool isNeedBid)
		{
			var printObject = _printObjectsManager.GetNeedBidPrintObjectsForProducer(producerId, isNeedBid);
			var printObjectShort = printObject.Select(po => PrintObjectToShort(po));

			return JsonConvert.SerializeObject(printObjectShort);
		}


		public string GetPrintObjectsByCompanyProducerJSON(
			long companyId,
			Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
		)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCompanyProducer(companyId, poStatus);
			var printObjectsShort = printObjects.Select(po => PrintObjectToShort(po));

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetPrintObjectByOrderJSON(long orderId)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByOrderId(orderId);
			var printObjectsShort = printObjects.Select(po => PrintObjectToShort(po));

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		#endregion

		private PrintObjectShort PrintObjectToShort(PrintObject printObject)
		{
			if (printObject == null)
			{
				throw new InvalidOperationException("Cannot cast null printobject");
			}
			return new PrintObjectShort()
			{
				Id = printObject.ID,
				ArtNo = printObject.ArticleNo,
				Name = printObject.Name,
				IsNeedBid = printObject.IsNeedBid
			};
		}

		private class PrintObjectShort
		{
			public long Id { get; set; }
			public string Name { get; set; }
			public long ArtNo { get; set; }
			public bool IsNeedBid { get; set; } //finish togglebid on UI
			public int  Quantity { get; set; }
		}

	}
}