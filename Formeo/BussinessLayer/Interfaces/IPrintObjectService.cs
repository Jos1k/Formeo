using Formeo.Models;
using Formeo.Models.HelperModels;
using System;
using System.Collections.Generic;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IPrintObjectsService
	{
		string GetPrintObjectsByCompanyCreatorJSON(long companyId);

		string GetPrintObjectsByIdsForCustomerJSON(IEnumerable<long> printObjectIds);

		string GetPrintObjectsByIdForProducerJSON(long printObjectId);

		string GetExclusivePrintObjectsByIdsForCompanyJSON(
			long companyId,
			IEnumerable<long> printObjectIdsToExlude);

		string GetNeedBidPrintObjectsForProducerJSON(string producerId, bool isNeedBid);

		string GetPrintObjectByOrderJSON(long orderId);

		string UploadProducts(IEnumerable<PrintObjectFileInfo> fileInfos);

		string AssignProducerToPrintObject(long producerCompanyId, long printObjectId);
	}
}
