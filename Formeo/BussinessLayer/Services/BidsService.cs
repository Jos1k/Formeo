using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.Services
{
	public class BidsService : IBidsService
	{
		IBidsManager _bidsManager;
		IUserManager _userManager;
		public BidsService(IBidsManager bidsManager, IUserManager userManager)
		{
			_userManager = userManager;
			_bidsManager = bidsManager;
		}

		#region IBidsService members

		public string CreateBidJSON(long printObjectId, decimal price)
		{
			ApplicationUser user = _userManager.GetCurrentUser();

			if (user == null)
			{
				throw new InvalidOperationException();
			}

			Bid newBid = _bidsManager.CreateBid(printObjectId, user.Company.ID, price);
			BidForProducerShort bshort = BidToForProducerShort(newBid);
			return JsonConvert.SerializeObject(bshort);
		}

		public string GetBidsForPrintObjectJSON(long printObjectId) 
		{
			IEnumerable<Bid> bids = _bidsManager.GetBidsForPrintObject(printObjectId);
			IEnumerable<BidForCustomerShort> bidsShort = bids.
				Select(
				bid =>
					BidToForCustomerShort(bid)
				).ToArray();
			return JsonConvert.SerializeObject(bidsShort);
		}

		#endregion


		#region Helpers

		private BidForCustomerShort BidToForCustomerShort(Bid bid)
		{
			if (bid == null)
			{
				return null;
			}
			BidForCustomerShort shortBid = new BidForCustomerShort();
			shortBid.ProducerCompanyName = bid.CompanyProducer.Name;
			shortBid.ProducerCompanyId = bid.CompanyProducer.ID;
			shortBid.Price = bid.Price;
			shortBid.PrintObjectId = bid.PrintObject.ID;
			shortBid.PrintObjectName = bid.PrintObject.Name;
			shortBid.ArtNo = bid.PrintObject.ArticleNo;
			shortBid.IsSelected = bid.PrintObject.CompanyProducer == null ? false : bid.PrintObject.CompanyProducer.ID == bid.CompanyProducer.ID;
			return shortBid;
		}

		private BidForProducerShort BidToForProducerShort(Bid bid)
		{
			if (bid == null)
			{
				return null;
			}
			BidForProducerShort shortBid = new BidForProducerShort();
			shortBid.CompanyCreatorName = bid.PrintObject.CompanyCreator.Name;
			shortBid.Price = bid.Price;
			shortBid.PrintObjectId = bid.PrintObject.ID;
			shortBid.PrintObjectName = bid.PrintObject.Name;
			return shortBid;
		}

		#endregion

		class BidForProducerShort
		{
			public string PrintObjectName { get; set; }
			public long PrintObjectId { get; set; }
			public string CompanyCreatorName { get; set; }
			public decimal Price { get; set; }
		}

		class BidForCustomerShort
		{
			public string PrintObjectName { get; set; }
			public long PrintObjectId { get; set; }
			public string ArtNo { get; set; }
			public decimal Price { get; set; }
			public long ProducerCompanyId { get; set; }
			public string ProducerCompanyName { get; set; }
			public bool IsSelected { get; set; }
		}
	}
}