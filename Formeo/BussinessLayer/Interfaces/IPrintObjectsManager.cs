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
		IEnumerable<PrintObject> GetPrintObjectsForUser(string userId);

		IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds);

		PrintObject GetPrintObjectById(long printObjectIds);
	}
}
