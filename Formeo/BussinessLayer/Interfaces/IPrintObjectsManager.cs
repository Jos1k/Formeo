using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IPrintObjectsManager
	{
		IEnumerable<PrintObject> GetPrintObjectsByCreatorCompany(long companyId);

		IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds);
		PrintObject GetPrintObjectById(long printObjectId);

		IEnumerable<PrintObject> GetPrintObjectsByOrderId(long orderId);

		IEnumerable<PrintObject> GetNeedBidPrintObjectsForProducer(string producerId, bool isNeedBid);

		IEnumerable<PrintObject> GetPrintObjectsByCompanyProducer(
			long companyId,
			Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
		);

		IEnumerable<PrintObject> GetPrintObjectsByOrder(long orderId);

		bool ToggleIsNeedBid(long printObjectId);

		void AssignProducerToPrintObject(long producerCompanyId, long printObjectId);

		PrintObject UploadPrintObject( string userId, string articleNo, string productName, string pathToFile, int printMaterialId );
	}
}
