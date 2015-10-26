using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface IBidsManager
	{
		/// <summary>
		/// This method creates Bid on behalf of users company
		/// </summary>
		/// <returns></returns>

		Bid CreateBid(long printObjectId, long companyId, decimal price, string currency);
		IReadOnlyList<Bid> GetBidsForPrintObject(long printObjectId);
		Bid GetSelecetdBidForPrintObject(long printObjectId);
	}
}
