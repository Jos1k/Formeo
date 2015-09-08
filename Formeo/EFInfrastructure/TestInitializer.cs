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
					Country = "UK",
					Name = "Company 1",
					OrgNumber = "OrgNumber1",
					TaxNumber = "TaxNumber1"
				},
				new Company()
				{
					Country = "US",
					Name = "Company 2",
					OrgNumber = "OrgNumber2",
					TaxNumber = "TaxNumber2"
				},
			};

			companies.ForEach(c => context.Company.Add(c));

			context.SaveChanges();

			#endregion

			#region Users

			List<User> users = new List<User>()
			{
				new User()
				{
					Country = "US",
					Email = "SomeEmail@somedomain.com",
					Name = "Gordon Freeman",
					Password = "NoHL3",
					ZipCode = "SomeZipCode",
					Adress = "New Mexico City, 21,32",
					UserType = UserType.Customer,
					Company = companies.ElementAt(0)
				},

				new User()
				{
					Country = "IT",
					Email = "SomeEmail@somedomain.com",
					Name = "Mario",
					Password = "ILikeMushrooms",
					ZipCode = "SomeZipCode Mario Edition",
					Adress = "NY, 50,50",
					UserType = UserType.Producer,
					Company = companies.ElementAt(0)
				},

				new User()
				{
					Country = "US",
					Email = "SomeEmail@somedomain.com",
					Name = "Dead pool",
					Password = "ILoveDeath",
					ZipCode = "SomeZipCode Crazy Edition",
					Adress = "Away from normal",
					UserType = UserType.Producer,
					Company = companies.ElementAt(1)
				},

				new User()
				{
					Email = "SomeEmail@somedomain.com",
					Name = "ADMIN",
					Password = "Obey",
					UserType = UserType.Admin
				}
			};


			users
				.Where(user => user.Company != null)
				.ToList()
				.ForEach(user => user.Company.Users.Add(user));

			users.ForEach(u => context.User.Add(u));

			context.SaveChanges();

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

			statuses.ForEach(s => context.Status.Add(s));

			context.SaveChanges();

			#endregion

			#region Projects
			List<Project> projects = new List<Project>()
			{
				new Project()
				{
					Name = "Order1",
					Customer = users.ElementAt(0),
					Producer = users.ElementAt(1),
					Status = statuses.ElementAt(0)
				},
				new Project()
				{
					Name = "Order2",
					Customer = users.ElementAt(0),
					Producer = users.ElementAt(2),
					Status = statuses.ElementAt(0)

				},
				new Project()
				{
					Name = "Order3",
					Customer = users.ElementAt(0),
					Status = statuses.ElementAt(0),
					
				}

			};

			projects.ForEach(o => context.Project.Add(o));

			context.SaveChanges();

			#endregion

			#region Bids

			List<Bid> bids = new List<Bid>()
			{
				new Bid() 
				{
					Price = 10,
					Project = projects.ElementAt(0),
					Producer = users.ElementAt(1)
				},
				new Bid ()
				{
					Price = 20,
					Project = projects.ElementAt(0),
					Producer = users.ElementAt(2)
				},
				new Bid 
				{
					Price = 30,
					Project = projects.ElementAt(0),
					Producer = users.ElementAt(2)
				},
				////
				new Bid() 
				{
					Price = 11,
					Project = projects.ElementAt(1),
					Producer = users.ElementAt(1)
				},
				new Bid ()
				{
					Price = 21,
					Project = projects.ElementAt(1),
					Producer = users.ElementAt(1)
				},
				new Bid 
				{
					Price = 31,
					Project = projects.ElementAt(1),
					Producer = users.ElementAt(2)
				}
			};

			bids.ForEach(bid => bid.Project.Bids.Add(bid));
			bids.ForEach(b => context.Bid.Add(b));

			context.SaveChanges();

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

			printMaterials.ForEach(pm => context.PrintMaterial.Add(pm));

			context.SaveChanges();

			#endregion

			#region OrderLines

			List<OrderLine> orderLines = new List<OrderLine>()
			{
				new OrderLine()
				{
					Name="OrderLine1",
					Project = projects.ElementAt(0),
					
				},

				new OrderLine()
				{
					Name="OrderLine2",
					Project = projects.ElementAt(1),
					

				},
				new OrderLine()
				{
					Name="OrderLine3",
					Project = projects.ElementAt(2)

				},
					new OrderLine()
				{
					Name="OrderLine4",
					Project = projects.ElementAt(2)

				}
			};

			orderLines.ForEach(o => context.OrderLine.Add(o));
			context.SaveChanges();

			#endregion

			#region PrintObjects

			List<PrintObject> printObjects = new List<PrintObject>()
			{
				new PrintObject()
				{
					Name = "PrintObject1",
					CadFile = "CadFile1",
					PropertiesSpecificationFile = "PropertiesSpecificationFile1",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = printMaterials.ElementAt(0),
					OrderLine = orderLines.ElementAt(0)
				},

				new PrintObject()
				{
					Name = "PrintObject2",
					CadFile = "CadFile2",
					PropertiesSpecificationFile = "PropertiesSpecificationFile2",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = printMaterials.ElementAt(1),
					OrderLine = orderLines.ElementAt(1)
				},

				new PrintObject()
				{
					Name = "PrintObject3",
					CadFile = "CadFile3",
					PropertiesSpecificationFile = "PropertiesSpecificationFile3",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = printMaterials.ElementAt(2),
					OrderLine = orderLines.ElementAt(2)


				},

				new PrintObject()
				{
					Name = "PrintObject4",
					CadFile = "CadFile4",
					PropertiesSpecificationFile = "PropertiesSpecificationFile4",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = printMaterials.ElementAt(3),
					OrderLine = orderLines.ElementAt(3)


				},

				new PrintObject()
				{
					Name = "PrintObject5",
					CadFile = "CadFile5",
					PropertiesSpecificationFile = "PropertiesSpecificationFile5",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = printMaterials.ElementAt(4),
					OrderLine = orderLines.ElementAt(2)

				}
			};

			printObjects.ForEach(po => context.PrintObject.Add(po));

			context.SaveChanges();

			#endregion

			projects.ElementAt(0).Bid = bids.ElementAt(1);

			context.SaveChanges();



		}
	}
}