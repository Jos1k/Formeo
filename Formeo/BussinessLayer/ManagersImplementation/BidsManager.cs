using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	public class BidsManager : IBidsManager
	{
		ApplicationDbContext _dbContext;
		IPrintObjectsManager _printObjectsManager;
		ICompaniesManager _companiesManager;
		public BidsManager(
			ApplicationDbContext dbCongtext,
			IPrintObjectsManager printObjectsManager,
			ICompaniesManager companiesManager
			)
		{
			_dbContext = dbCongtext;
			_printObjectsManager = printObjectsManager;
			_companiesManager = companiesManager;
		}

		public Bid CreateBid(long printObjectId, string userId, decimal price)
		{
			PrintObject printObject = _printObjectsManager.GetPrintObjectById(printObjectId);
			Company bidCompany = _companiesManager.GetCompanyByUserId(userId);
			if (printObject == null)
			{
				return null;
			}

			Bid newBid = new Bid();
			newBid.PrintObject = printObject;
			newBid.Price = price;
			newBid.CompanyProducer = bidCompany;

			_dbContext.Bids.Add(newBid);
			_dbContext.SaveChanges();
			_dbContext.Entry(newBid.PrintObject).Reload();
			_dbContext.Entry(newBid.CompanyProducer).Reload();

			return newBid;
		}

		public Bid CreateBid(long printObjectId, long companyId, decimal price)
		{
			PrintObject printObject = _printObjectsManager.GetPrintObjectById(printObjectId);
			Company bidCompany = _companiesManager.GetCompanyById(companyId);
			if (printObject == null)
			{
				return null;
			}

			Bid newBid = new Bid();
			newBid.PrintObject = printObject;
			newBid.Price = price;
			newBid.CompanyProducer = bidCompany;

			_dbContext.Bids.Add(newBid);
			_dbContext.SaveChanges();

			return newBid;
		}

		public IReadOnlyList<Bid> GetBidsForPrintObject(long printObjectId)
		{
			return _dbContext.Bids
					.Where(bid => bid.PrintObject.ID == printObjectId)
					.ToArray(); //to array because connection will be closed beforehand
		}

		public Bid GetSelecetdBidForPrintObject(long printObjectId) 
		{
			PrintObject printObject = _printObjectsManager.GetPrintObjectById(printObjectId);
			if (printObject==null||printObject.CompanyProducer == null) 
			{
				return null;
			}

			return _dbContext.Bids
				.Where(bid => 
						bid.CompanyProducer.ID == printObject.CompanyProducer.ID 
						&& bid.PrintObject.ID == printObjectId)
					.SingleOrDefault();
		}
	}
}