using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	public static class StaticData
	{
		public static class RoleNames
		{
			public const string Admin = "Admin";
			public const string Customer = "Customer";
			public const string Producer = "Producer";
		}

		public static class StatusNames 
		{
			public const string InQueue = "InQueue";//new order
			public const string Preparing = "Preparing";
			public const string Running = "Running";
			public const string Finished = "Finished";
			public const string Canceled = "Canceled";
			public const string NotAccepted = "Not Accepted";
		}
	}
}