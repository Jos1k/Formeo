using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Formeo.Model;


namespace Formeo.EFInfrastructure
{
	public class TestInitializer :
						DropCreateDatabaseIfModelChanges<FormeoDBContext>
	{
		protected override void Seed(FormeoDBContext context)
		{
			#region Companies
			List<Company> companies = new List<Company>()
			{
				new Company()
				{
					ID = 1,
					Country = "UK",
					Name = "Company 1",
					OrgNumber = "OrgNumber1",
					TaxNumber = "TaxNumber1"
				},
				new Company()
				{
					ID = 2,
					Country = "US",
					Name = "Company 2",
					OrgNumber = "OrgNumber2",
					TaxNumber = "TaxNumber2"
				},
			};

			companies.ForEach(company => context.Company.Add(company));
			#endregion

			#region Orders
			List<Order> orders = new List<Order>()
			{
				new Order()
				{
					ID = 1,
					Name = "Order1"
				},
				new Order()
				{
					ID = 2,
					Name = "Order2"
				}

			};

			orders.ForEach(order => context.Order.Add(order));

			#endregion

			#region OrderLines

			List<OrderLine> orderLines = new List<OrderLine>()
			{
				new OrderLine()
				{
					ID=1,
					Name="OrderLine1"
				},

				new OrderLine()
				{
					ID=2,
					Name="OrderLine2"
				},
				new OrderLine()
				{
					ID=3,
					Name="OrderLine3"
				}
			};

			#endregion

			#region PrintMaterial

			List<PrintMaterial> printMaterials = new List<PrintMaterial>()
			{
				new PrintMaterial()
				{
					ID = 1,
					Name = "PrintMaterial1",
				
				},

				new PrintMaterial()
				{
					ID = 2,
					Name = "PrintMaterial2",
				}

			};

			#endregion


			#region PrintObjects

			List<PrintObject> printObjects = new List<PrintObject>()
			{
				new PrintObject()
				{
					ID = 1,
					Name = "PrintObject1",
					CadFile = "CadFile1",
					PropertiesSpecificationFile = "PropertiesSpecificationFile1",
					CustomerArticleNumber = "CustomerArticleNumber"
				},

				new PrintObject()
				{
					ID = 2,
					Name = "PrintObject2",
					CadFile = "CadFile2",
					PropertiesSpecificationFile = "PropertiesSpecificationFile2",
					CustomerArticleNumber = "CustomerArticleNumber"
				}

			};

			#endregion

			#region Statuses

			List<Status> statuses = new List<Status>() 
			{
				new Status()
				{
					ID=1,
					Name = "Status1"
				},

				new Status()
				{
					ID=2,
					Name = "Status2"
				},

				new Status()
				{
					ID=3,
					Name = "Status3"
				}
			};
			#endregion

			#region Users

			List<User> users = new  List<User>()
			{
				new User()
				{
					ID = 1,
					Country = "US",
					Email = "SomeEmail@somedomain.com",
					Name = "Gordon Freeman",
					Password = "NoHL3",
					ZipCode = "SomeZipCode",
					Adress = "New Mexico City, 21,32",
				},

				new User()
				{
					ID = 1,
					Country = "IT",
					Email = "SomeEmail@somedomain.com",
					Name = "Mario",
					Password = "ILikeMushrooms",
					ZipCode = "SomeZipCode Mario Edition",
					Adress = "NY, 50,50",
				}
			}
			#endregion
		}
	}
}