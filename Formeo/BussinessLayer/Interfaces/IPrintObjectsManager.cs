using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IPrintObjectsManager
	{
		IEnumerable<PrintObject> GetPrintObjectsByCreatorCompany(long companyId);

		IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds);
		PrintObject GetPrintObjectById(long printObjectId);

		IEnumerable<PrintObject> GetPrintObjectByOrderId(long orderId);

		IEnumerable<PrintObject> GetNeedBidPrintObjectsForProducer(string producerId, bool isNeedBid);

		IEnumerable<PrintObject> GetPrintObjectsByOrder(long orderId);

		bool ToggleIsNeedBid(long printObjectId);

		PrintObject AssignProducerToPrintObject(long producerCompanyId, long printObjectId);

		//PrintObject UploadPrintObject( string userId, string articleNo, string productName, string pathToFile, int printMaterialId );

		IEnumerable<PrintObject> UploadPrintObjects(IEnumerable<Models.HelperModels.PrintObjectFileInfo> fileInfos);
		string GetPrintObjectFilePath( long printObjectId );
	}
}
