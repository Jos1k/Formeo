using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation {
	public class BidsManager : IBidsManager {
		ApplicationDbContext _dbContext;
		IPrintObjectsManager _printObjectsManager;
		ICompaniesManager _companiesManager;
		public BidsManager(
			ApplicationDbContext dbCongtext,
			IPrintObjectsManager printObjectsManager,
			ICompaniesManager companiesManager
			) {
			_dbContext = dbCongtext;
			_printObjectsManager = printObjectsManager;
			_companiesManager = companiesManager;
		}



		public Bid CreateBid(string producerName, long printObjectId, long companyId, decimal price, string currency ) {
			PrintObject printObject = _printObjectsManager.GetPrintObjectById( printObjectId );
			Company bidCompany = _companiesManager.GetCompanyById( companyId );
			if( printObject == null ) {
				return null;
			}

			Bid newBid = new Bid();
			newBid.PrintObject = printObject;
			newBid.Price = Math.Round( price, 2 );
			newBid.CompanyProducer = bidCompany;
			newBid.Currency = currency;

			_dbContext.Bids.Add( newBid );
			_dbContext.SaveChanges();

			List<string> customersEmail = printObject
				.CompanyCreator
				.Users
				.Select( customer => customer.Email )
				.ToList();

			foreach( string producerEmail in customersEmail ) {
				StaticData.SendEmail(
					producerEmail,
					"You have a new bid",
					string.Format(
						"Hello!\n\nYou have a new received bid in Formeo from {0} for {1}",
						producerName,
						printObject.ArticleNo
					)
				);
			}

			return newBid;
		}

		public IReadOnlyList<Bid> GetBidsForPrintObject( long printObjectId ) {
			return _dbContext.Bids
					.Where( bid => bid.PrintObject.ID == printObjectId )
					.ToArray(); //to array because connection will be closed beforehand
		}

		public Bid GetSelecetdBidForPrintObject( long printObjectId ) {
			PrintObject printObject = _printObjectsManager.GetPrintObjectById( printObjectId );
			if( printObject == null || printObject.CompanyProducer == null ) {
				return null;
			}

			return _dbContext.Bids
				.Where( bid =>
						bid.CompanyProducer.ID == printObject.CompanyProducer.ID
						&& bid.PrintObject.ID == printObjectId )
					.SingleOrDefault();
		}
	}
}