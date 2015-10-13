﻿using Formeo.Models;
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

		string GetPrintObjectsByCompanyProducerJSON(
			long companyId,
			Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
		);

		string GetPrintObjectByOrderJSON(long orderId);
	}
}
