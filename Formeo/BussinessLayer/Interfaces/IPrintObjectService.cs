using Formeo.Models;
using System;
using System.Collections.Generic;
namespace Formeo.BussinessLayer
{
	public interface IPrintObjectService
	{
		string GetPrintObjectsForUserJSON(ApplicationUser user);

		string GetPrintObjectsByIdsJSON(IEnumerable<long> printObjectIds);

		string GetExclusivePrintObjectsByIdsForUserJSON(
			ApplicationUser user,
			IEnumerable<long> printObjectIdsToExlude);
	}
}
