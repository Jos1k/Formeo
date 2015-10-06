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

		private ApplicationDbContext _dbContext;

		public PrintObjectsManager( ApplicationDbContext dbContext ) {
			_dbContext = dbContext;
		}

		#region  IPrintObjectsManager members
		public IEnumerable<PrintObject> GetPrintObjectsForUser(string userId)
		{
			List<PrintObject> printObjects = _dbContext.PrintObjects.Where(
				po => po.CustomerCreator.Id == userId)
				.ToList();

			return printObjects;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds)
		{
			List<PrintObject> printObjects = _dbContext.PrintObjects.Where(
				po => printObjectIds.Contains(po.ID)
			).ToList();
			return printObjects;
		}

		public PrintObject GetPrintObjectById(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(
				po => po.ID == printObjectId
			).FirstOrDefault();
			return printObject;
		}

		#endregion




	}
}