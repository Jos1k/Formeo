using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Formeo.Models {
	public static class StaticData {
		public static class RoleNames {
			public const string Admin = "Admin";
			public const string Customer = "Customer";
			public const string Producer = "Producer";
		}

		public enum OrderStatusEnum {
			InProgress = 1,
			Delivered
		}

		public enum PrintObjectStatusEnum {
			InQueue = 1,
			Producing,
			Delivered
		}

		public static string GetOrderStatusName( this OrderStatusEnum status ) {
			string statusRes;
			switch( status ) {
				case OrderStatusEnum.InProgress: {
						statusRes = "In Progres";
						break;
					}
				case OrderStatusEnum.Delivered: {
						statusRes = "Delivered";
						break;
					}
				default: {
						statusRes = "Unknown status";
						break;
					}
			}
			return statusRes;
		}

		public static string GetPrintObjectStatusName( this PrintObjectStatusEnum status ) {
			switch( status ) {
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

		public static void SendEmail( string toEmail, string subject, string body ) {

			var fromAddress = new MailAddress( ConfigurationManager.AppSettings["NotificationEmail"] );
			var toAddress = new MailAddress( toEmail );
			string fromPassword = ConfigurationManager.AppSettings["NotificationPassword"];
			string _subject = subject;
			string _body = body;

			var smtp = new SmtpClient {
				Host = ConfigurationManager.AppSettings["NotificationSMTP"],
				Port = int.Parse( ConfigurationManager.AppSettings["NotificationPort"] ),
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				//UseDefaultCredentials = false,
				Credentials = new NetworkCredential( ConfigurationManager.AppSettings["NotificationLogin"], fromPassword )
			};

			Thread T1 = new Thread( delegate() {
				using( var message = new MailMessage( fromAddress, toAddress ) {
					Subject = _subject,
					Body = _body
				} ) {
					{
						smtp.Send( message );
					}
				}
			} );

			T1.Start();
		}
	}
}