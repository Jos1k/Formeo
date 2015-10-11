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
		ApplicationDbContext _dbCongtext;
		IPrintObjectsManager _printObjectsManager;
		ICompaniesManager _companiesManager;
		public BidsManager(
			ApplicationDbContext dbCongtext,
			IPrintObjectsManager printObjectsManager,
			ICompaniesManager companiesManager
			)
		{
			_dbCongtext = dbCongtext;
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

			_dbCongtext.Bids.Add(newBid);
			_dbCongtext.SaveChanges();

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

			_dbCongtext.Bids.Add(newBid);
			_dbCongtext.SaveChanges();

			return newBid;
		}

	}
}