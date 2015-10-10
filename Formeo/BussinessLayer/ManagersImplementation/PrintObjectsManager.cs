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

		public PrintObjectsManager(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region  IPrintObjectsManager members
		public IEnumerable<PrintObject> GetPrintObjectsByCreatorCompany(long comapnyId)
		{
			List<PrintObject> printObjects =
				_dbContext
					.PrintObjects
					.Where(
						po => po.CompanyCreator.ID == comapnyId)
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

		public PrintObject GetPrintObjectsById(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(
				po => po.ID == printObjectId
			).Single();
			return printObject;
		}


		public PrintObject GetPrintObjectById(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(
				po => po.ID == printObjectId
			).FirstOrDefault();
			return printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrderId(long orderId)
		{
			return from printObject in _dbContext.PrintObjects
				   join relation in _dbContext.ProjectsInfo
				   on printObject.ID equals relation.PrintObjectId
				   where relation.ProjectId == orderId
				   select printObject;
		}

		public IEnumerable<PrintObject> GetNeedBidPrintObjectsForProducer(string producerId, bool isNeedBid)
		{
			IEnumerable<PrintObject> printObjectsResult =
				from printObject in _dbContext.PrintObjects
				where
					!(from bid in _dbContext.Bids
						where bid.Producer.Id == producerId
						select bid.PrintObject.ID)
					.Contains(printObject.ID)
					&& printObject.IsNeedBid == isNeedBid
				select printObject;

			return printObjectsResult;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByCompanyProducer(
				long companyId,
				Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
			)
		{
			return from printObject in _dbContext.PrintObjects
				   join projectInfo in _dbContext.ProjectsInfo
				   on printObject.ID equals projectInfo.PrintObject.ID
				   where projectInfo.CompanyProducer.ID == companyId
				   select printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrder(long orderId)
		{
			return _dbContext.PrintObjects.Join(
					_dbContext.ProjectsInfo,
					po => po.ID,
					pi => pi.PrintObjectId,
					(po, pi) => po
				);
		}

		public bool ToggleIsNeedBid(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(po => po.ID == printObjectId).SingleOrDefault();
			if (printObject == null)
			{
				throw new InvalidOperationException("printObjectId is Invalid");
			}
			printObject.IsNeedBid = !printObject.IsNeedBid;
			_dbContext.SaveChanges();

			return printObject.IsNeedBid;
		}

		#endregion


	}
}