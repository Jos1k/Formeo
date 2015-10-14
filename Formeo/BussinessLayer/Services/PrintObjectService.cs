using Formeo.BussinessLayer.Interfaces;
using Formeo.BussinessLayer.ManagersImplementation;
using Formeo.Models;
using Formeo.Models.HelperModels;
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
		IBidsManager _bidsManager;
		public PrintObjectService(IPrintObjectsManager printObjectsManager, IBidsManager bidsManager)
		{
			_printObjectsManager = printObjectsManager;
			_bidsManager = bidsManager;
		}

		#region IPrintObjectService Members
		public string GetPrintObjectsByIdsForCustomerJSON(IEnumerable<long> printObjectIds)
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

			IReadOnlyList<PrintObjectBaseShort> printObjectsShort =
				printObjects.Select(po =>
					PrintObjectForCustomerToShort(po))
					.ToArray();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetPrintObjectsByIdForProducerJSON(long printObjectId)
		{
			var printObject = _printObjectsManager.GetPrintObjectById(printObjectId);

			if (printObject == null)
			{
				return string.Empty;
			}

			PrintObjectForProducerShort printObjectShort = PrintObjectForProducerToShort(printObject);

			return JsonConvert.SerializeObject(printObjectShort);
		}

		public string GetPrintObjectsByCompanyCreatorJSON(long companyId)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCreatorCompany(companyId);
			var printObjectsShort = printObjects.Select(
					po => PrintObjectForCustomerToShort(po)
				).ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		/// <summary>
		/// For customers only
		/// </summary>
		public string GetExclusivePrintObjectsByIdsForCompanyJSON(long companyId, IEnumerable<long> printObjectIdsToExlude)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCreatorCompany(companyId);

			var printObjectsShort = printObjects
				.Where(po => !printObjectIdsToExlude.Contains(po.ID))
				.Select(po =>
						PrintObjectForCustomerToShort(po)
					)
				.ToList();

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string GetNeedBidPrintObjectsForProducerJSON(string producerId, bool isNeedBid)
		{
			var printObject = _printObjectsManager.GetNeedBidPrintObjectsForProducer(producerId, isNeedBid);
			var printObjectShort = printObject.Select(po => PrintObjectForProducerToShort(po));

			return JsonConvert.SerializeObject(printObjectShort);
		}

		public string GetPrintObjectsByCompanyProducerJSON(
			long companyId,
			Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
		)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByCompanyProducer(companyId, poStatus);
			var printObjectsShort = printObjects.Select(po => PrintObjectForProducerToShort(po));

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		/// <summary>
		/// for customers
		/// </summary>
		public string GetPrintObjectByOrderJSON(long orderId)
		{
			var printObjects = _printObjectsManager.GetPrintObjectsByOrderId(orderId);
			var printObjectsShort = printObjects.Select(po => PrintObjectForCustomerToShort(po));

			return JsonConvert.SerializeObject(printObjectsShort);
		}


		public string UploadProducts(IEnumerable<PrintObjectFileInfo> fileInfos)
		{
			if (fileInfos == null || fileInfos.Count() == 0)
			{
				return string.Empty;
			}

			IEnumerable<PrintObject> createdPrintObjects = _printObjectsManager.UploadPrintObjects(fileInfos);

			//here - even producers should get view as customers 
			IEnumerable<PrintObjectForCustomerShort> printObjectsShort =
				createdPrintObjects.Select(
					po => PrintObjectForCustomerToShort(po)
				);

			return JsonConvert.SerializeObject(printObjectsShort);
		}

		public string AssignProducerToPrintObject(long producerCompanyId, long printObjectId)
		{
			PrintObject printObject =
				_printObjectsManager.AssignProducerToPrintObject(producerCompanyId, printObjectId);

			PrintObjectForCustomerShort poShort = PrintObjectForCustomerToShort(printObject);
			return JsonConvert.SerializeObject(poShort);
		}

		#endregion

		private PrintObjectForProducerShort PrintObjectForProducerToShort(PrintObject printObject, int quantity = 0)
		{
			if (printObject == null)
			{
				throw new InvalidOperationException("Cannot cast null printobject");
			}

			bool hasBidsForPrintObject = printObject.Bids != null && printObject.Bids.ToArray().Length > 0;

			return new PrintObjectForProducerShort()
			{
				Id = printObject.ID,
				ArtNo = printObject.ArticleNo,
				Name = printObject.Name,
				CompanyName = printObject.CompanyCreator.Name,
				Quantity = quantity,
			};
		}

		private PrintObjectForCustomerShort PrintObjectForCustomerToShort(PrintObject printObject)
		{
			if (printObject == null)
			{
				throw new InvalidOperationException("Cannot cast null printobject");
			}

			bool hasBidsForPrintObject = printObject.Bids != null && printObject.Bids.Count > 0;

			long? selectedCompanyProducerID = null;
			string selectedProducerCompanyName = null;
			decimal? selectedPrice = null;

			if (printObject.CompanyProducer != null)
			{
				selectedProducerCompanyName = printObject.CompanyProducer == null ? null : printObject.CompanyProducer.Name;
				selectedCompanyProducerID = printObject.CompanyProducer == null ? (long?)null : printObject.CompanyProducer.ID;

				Bid selectedBid = _bidsManager.GetSelecetdBidForPrintObject(printObject.ID);
				selectedPrice = selectedBid == null ? (decimal?)null : selectedBid.Price;
			}

			return new PrintObjectForCustomerShort()
			{
				Id = printObject.ID,
				ArtNo = printObject.ArticleNo,
				Name = printObject.Name,
				IsNeedBid = printObject.IsNeedBid,
				HasBids = hasBidsForPrintObject,
				Quantity = 1,
				SelecterProducerCompanyId = selectedCompanyProducerID,
				CompanyName = selectedProducerCompanyName,
				CurrentPrice = selectedPrice
			};

		}

		#region Classes
		private class PrintObjectBaseShort
		{
			public long Id { get; set; }
			public string Name { get; set; }
			public long ArtNo { get; set; }
			public int Quantity { get; set; }
		}

		private class PrintObjectForCustomerShort : PrintObjectBaseShort
		{
			//producerCompanyName
			public string CompanyName { get; set; }
			public bool IsNeedBid { get; set; }
			public bool HasBids { get; set; }
			public long? SelecterProducerCompanyId { get; set; }
			public decimal? CurrentPrice { get; set; }

		}

		private class PrintObjectForProducerShort : PrintObjectBaseShort
		{
			//Customer-creator name
			public string CompanyName { get; set; }
		}
		#endregion




	}
}