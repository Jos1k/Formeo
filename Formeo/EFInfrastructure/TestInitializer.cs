using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Formeo.Model;


namespace Formeo.EFInfrastructure
{
	public class TestInitializer :
						DropCreateDatabaseAlways<FormeoDBContext>
	{
		protected override void Seed(FormeoDBContext context)
		{
			#region Companies
			List<Company> companies = new List<Company>()
			{
				new Company()
				{
					//ID = 1,
					Country = "UK",
					Name = "Company 1",
					OrgNumber = "OrgNumber1",
					TaxNumber = "TaxNumber1"
				},
				new Company()
				{
					//ID = 2,
					Country = "US",
					Name = "Company 2",
					OrgNumber = "OrgNumber2",
					TaxNumber = "TaxNumber2"
				},
			};

			#endregion

			#region Orders
			List<Project> projects = new List<Project>()
			{
				new Project()
				{
					//ID = 1,
					Name = "Order1"
				},
				new Project()
				{
					//ID = 2,
					Name = "Order2"
				},
				new Project()
				{
					//ID = 2,
					Name = "Order3"
				}

			};


			#endregion

			#region OrderLines

			List<OrderLine> orderLines = new List<OrderLine>()
			{
				new OrderLine()
				{
					//ID=1,
					Name="OrderLine1"
				},

				new OrderLine()
				{
					//ID=2,
					Name="OrderLine2"
				},
				new OrderLine()
				{
					//ID=3,
					Name="OrderLine3"
				},
				new OrderLine()
				{
					//ID=4,
					Name="OrderLine4"
				},
				new OrderLine()
				{
					//ID=5,
					Name="OrderLine5"
				}
			};

			#endregion

			#region PrintMaterial

			List<PrintMaterial> printMaterials = new List<PrintMaterial>()
			{
				new PrintMaterial()
				{
					Name = "PrintMaterial1",
				
				},

				new PrintMaterial()
				{
					Name = "PrintMaterial2",
				},
				new PrintMaterial()
				{
					Name = "PrintMaterial3",
				},
				new PrintMaterial()
				{
					Name = "PrintMaterial4",
				},
				new PrintMaterial()
				{
					Name = "PrintMaterial5",
				}

			};

			#endregion


			#region PrintObjects

			List<PrintObject> printObjects = new List<PrintObject>()
			{
				new PrintObject()
				{
					//ID = 1,
					Name = "PrintObject1",
					CadFile = "CadFile1",
					PropertiesSpecificationFile = "PropertiesSpecificationFile1",
					CustomerArticleNumber = "CustomerArticleNumber"
				},

				new PrintObject()
				{
					//ID = 2,
					Name = "PrintObject2",
					CadFile = "CadFile2",
					PropertiesSpecificationFile = "PropertiesSpecificationFile2",
					CustomerArticleNumber = "CustomerArticleNumber"
				},

				new PrintObject()
				{
					//ID = 3,
					Name = "PrintObject3",
					CadFile = "CadFile3",
					PropertiesSpecificationFile = "PropertiesSpecificationFile3",
					CustomerArticleNumber = "CustomerArticleNumber"
				},

				new PrintObject()
				{
					//ID = 4,
					Name = "PrintObject4",
					CadFile = "CadFile4",
					PropertiesSpecificationFile = "PropertiesSpecificationFile4",
					CustomerArticleNumber = "CustomerArticleNumber"
				},

				new PrintObject()
				{
					//ID = 5,
					Name = "PrintObject5",
					CadFile = "CadFile5",
					PropertiesSpecificationFile = "PropertiesSpecificationFile5",
					CustomerArticleNumber = "CustomerArticleNumber"
				}

			};

			#endregion

			#region Statuses

			List<Status> statuses = new List<Status>() 
			{
				new Status()
				{
					//ID=1,
					Name = "Status1"
				},

				new Status()
				{
					//ID=2,
					Name = "Status2"
				},

				new Status()
				{
					//ID=3,
					Name = "Status3"
				}
			};
			#endregion

			#region Users

			List<User> users = new List<User>()
			{
				new User()
				{
					//ID = 1,
					Country = "US",
					Email = "SomeEmail@somedomain.com",
					Name = "Gordon Freeman",
					Password = "NoHL3",
					ZipCode = "SomeZipCode",
					Adress = "New Mexico City, 21,32",
					UserType = UserType.Customer
				},

				new User()
				{
					////ID = 2,
					Country = "IT",
					Email = "SomeEmail@somedomain.com",
					Name = "Mario",
					Password = "ILikeMushrooms",
					ZipCode = "SomeZipCode Mario Edition",
					Adress = "NY, 50,50",
					UserType = UserType.Producer
				},

				new User()
				{
					//ID = 3,
					Country = "US",
					Email = "SomeEmail@somedomain.com",
					Name = "Dead pool",
					Password = "ILoveDeath",
					ZipCode = "SomeZipCode Crazy Edition",
					Adress = "Away from normal",
					UserType = UserType.Producer
				},

				new User()
				{
					//ID = 4,
					Email = "SomeEmail@somedomain.com",
					Name = "ADMIN",
					Password = "Obey",
					UserType = UserType.Admin
				}
			};



			#endregion


			#region Bids

			List<Bid> bids = new List<Bid>()
			{
				new Bid() 
				{
					Price = 10,
				},
				new Bid ()
				{
					Price = 20
				},
				new Bid 
				{
					Price = 30
				},
				new Bid 
				{
					Price = 40
				},
				new Bid 
				{
					Price = 50
				}
			};

			#endregion

			#region Links

			companies.ElementAt(0).Users = new List<User>();
			companies.ElementAt(1).Users = new List<User>();

			companies.ElementAt(0).Users.Add(users.ElementAt(0));
			companies.ElementAt(0).Users.Add(users.ElementAt(1));
			companies.ElementAt(1).Users.Add(users.ElementAt(2));
			//////////////////

			projects.ElementAt(0).OrderLines = new List<OrderLine>();
			projects.ElementAt(1).OrderLines = new List<OrderLine>();
			projects.ElementAt(2).OrderLines = new List<OrderLine>();

			projects.ElementAt(0).OrderLines.Add(orderLines.ElementAt(0));
			projects.ElementAt(0).OrderLines.Add(orderLines.ElementAt(1));
			projects.ElementAt(0).OrderLines.Add(orderLines.ElementAt(2));
			projects.ElementAt(1).OrderLines.Add(orderLines.ElementAt(3));
			projects.ElementAt(2).OrderLines.Add(orderLines.ElementAt(4));

			projects.ElementAt(0).Customer = users.ElementAt(0);
			projects.ElementAt(0).Producer = users.ElementAt(1);

			projects.ElementAt(1).Customer = users.ElementAt(0);
			projects.ElementAt(1).Producer = users.ElementAt(2);

			projects.ElementAt(2).Customer = users.ElementAt(0);

			projects.ElementAt(0).Status = statuses.ElementAt(0);
			projects.ElementAt(1).Status = statuses.ElementAt(1);
			projects.ElementAt(2).Status = statuses.ElementAt(2);


			

			//////////////

			orderLines.ElementAt(0).PrintObject = printObjects.ElementAt(0);
			orderLines.ElementAt(1).PrintObject = printObjects.ElementAt(1);
			orderLines.ElementAt(2).PrintObject = printObjects.ElementAt(2);
			orderLines.ElementAt(3).PrintObject = printObjects.ElementAt(3);
			orderLines.ElementAt(4).PrintObject = printObjects.ElementAt(4);

			///////////

			bids.ElementAt(0).Producer = users.ElementAt(1);
			bids.ElementAt(1).Producer = users.ElementAt(2);
			bids.ElementAt(2).Producer = users.ElementAt(1);
			bids.ElementAt(3).Producer = users.ElementAt(2);
			bids.ElementAt(4).Producer = users.ElementAt(1);

			bids.ElementAt(0).Project = projects.ElementAt(1);
			bids.ElementAt(1).Project = projects.ElementAt(2);
			bids.ElementAt(2).Project = projects.ElementAt(1);
			bids.ElementAt(3).Project = projects.ElementAt(2);
			bids.ElementAt(4).Project = projects.ElementAt(1);


			#endregion

			companies.ForEach(c => context.Company.Add(c));
			bids.ForEach(b => context.Bid.Add(b));
			projects.ForEach(o => context.Project.Add(o));
			orderLines.ForEach(o => context.OrderLine.Add(o));
			users.ForEach(u => context.User.Add(u));
			statuses.ForEach(s => context.Status.Add(s));

			//2 save changes are neccessary
			context.SaveChanges();

			projects.ElementAt(0).Bids = new List<Bid>();
			projects.ElementAt(0).Bids.Add(bids.ElementAt(0));
			projects.ElementAt(0).Bids.Add(bids.ElementAt(1));
			projects.ElementAt(0).Bids.Add(bids.ElementAt(2));

			projects.ElementAt(0).Bid = bids.ElementAt(1);

			context.SaveChanges();



		}
	}
}