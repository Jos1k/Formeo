using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	public class PrintObjectsManager : IPrintObjectsManager
	{
		private ApplicationDbContext DbContext { get { return new ApplicationDbContext(); } }

		#region  IPrintObjectsManager members
		public IEnumerable<PrintObject> GetPrintObjectsForUser(ApplicationUser user)
		{
			List<PrintObject> printObjects = DbContext.PrintObjects.Where(
				po => po.CustomerCreator.Id == user.Id)
				.ToList();

			return printObjects;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds)
		{
			List<PrintObject> printObjects = DbContext.PrintObjects.Where(
				po => printObjectIds.Contains(po.ID)
			).ToList();
			return printObjects;
		}

		#endregion



	
	}
}