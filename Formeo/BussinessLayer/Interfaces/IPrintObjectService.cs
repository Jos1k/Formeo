using Formeo.Models;
using System;
using System.Collections.Generic;
namespace Formeo.BussinessLayer
{
	public interface IPrintObjectService
	{
		string GetPrintObjectsForUserJSON(string userId);

		string GetPrintObjectsByIdsJSON(IEnumerable<long> printObjectIds);

		string GetExclusivePrintObjectsByIdsForUserJSON(
			string userId,
			IEnumerable<long> printObjectIdsToExlude);
	}
}
