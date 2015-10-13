using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IBidsService
	{
		string CreateBidJSON(long printObjectId, decimal price);

		string GetBidsForPrintObjectJSON(long printObjectId);
	}
}