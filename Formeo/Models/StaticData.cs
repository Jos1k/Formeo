﻿using System;
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

		public enum OrderStatusEnum
		{
			InProgress = 1,
			Delivered
		}

		public enum PrintObjectStatusEnum
		{
			InQueue = 1,
			Producing,
			Delivered
		}

		public static string GetOrderStatusName(this OrderStatusEnum status)
		{
			string statusRes;
			switch (status)
			{
				case OrderStatusEnum.InProgress:
					{
						statusRes = "In Progres";
						break;
					}
				case OrderStatusEnum.Delivered:
					{
						statusRes = "Delivered";
						break;
					}
				default:
					{
						statusRes = "Unknown status";
						break;
					}
			}
			return statusRes;
		}

		public static string GetPrintObjectStatusName(this PrintObjectStatusEnum status) 
		{
			switch (status)
			{
				case PrintObjectStatusEnum.InQueue:
					return "In queue";
				case PrintObjectStatusEnum.Producing:
					return "In Production";
				case PrintObjectStatusEnum.Delivered:
					return "Delivered";
				default:
					return "Status unknown";
			}
		}
	}
}