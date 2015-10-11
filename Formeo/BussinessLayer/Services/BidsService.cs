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
			BidShort bshort = BidToShort(newBid);
			return JsonConvert.SerializeObject(bshort);
		}

		#endregion


		#region Helpers

		private BidShort BidToShort(Bid bid)
		{
			if (bid == null)
			{
				return null;
			}
			BidShort shortBid = new BidShort();
			shortBid.CompanyCreatorName = bid.PrintObject.CompanyCreator.Name;
			shortBid.Price = bid.Price;
			shortBid.PrintObjectId = bid.PrintObject.ID;
			shortBid.PrintObjectName = bid.PrintObject.Name;
			return shortBid;
		}

		#endregion

		class BidShort
		{
			public string PrintObjectName { get; set; }
			public long PrintObjectId { get; set; }
			public string CompanyCreatorName { get; set; }
			public decimal Price { get; set; }
		}
	}
}