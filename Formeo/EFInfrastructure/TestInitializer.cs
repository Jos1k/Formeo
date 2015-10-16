using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Formeo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.IO;


namespace Formeo.EFInfrastructure
{
	public class TestInitializer :
						DropCreateDatabaseIfModelChanges<ApplicationDbContext>
	{
		protected override void Seed(ApplicationDbContext context)
		{

			ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

			RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

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

			companies.ForEach(c => context.Companies.Add(c));

			context.SaveChanges();

			#endregion

			#region Roles

			List<IdentityRole> roles = new List<IdentityRole>() 
			{
				new IdentityRole()
				{
					Name =  StaticData.RoleNames.Admin
				},
				new IdentityRole()
				{
					Name =  StaticData.RoleNames.Producer
				},
				new IdentityRole()
				{
					Name = StaticData.RoleNames.Customer
				}
			};

			roles.ForEach(role => roleManager.Create(role));

			#endregion

			#region Users

			List<ApplicationUser> users = new List<ApplicationUser>()
			{
				new ApplicationUser()
				{
					Country = "US",
					Email = "SomeEmail1@somedomain.com",
					UserName = "Gordon_Freeman",
					ZipCode = "SomeZipCode",
					Adress = "New Mexico City, 21,32",
					Company = companies.ElementAt(0)
				},

				new ApplicationUser()
				{
					Country = "IT",
					Email = "SomeEmail2@somedomain.com",
					UserName = "Mario",
					ZipCode = "SomeZipCode Mario Edition",
					Adress = "NY, 50,50",
					Company = companies.ElementAt(0)
				},

				new ApplicationUser()
				{
					Country = "US",
					Email = "SomeEmail3@somedomain.com",
					UserName = "Dead_pool",
					ZipCode = "SomeZipCode Crazy Edition",
					Adress = "Away from normal",
					Company = companies.ElementAt(1)
				},

				new ApplicationUser()
				{
					Email = "SomeEmail4@somedomain.com",
					UserName = "ADMIN",
				}
			};

			int i = 0;

			//users.ForEach(user => userManager.Create(user, string.Format("{0},{1}", "Formeo", ++i)));

			foreach (var user in users)
			{

				IdentityResult result = userManager.Create(user, user.Email);

				while (!result.Succeeded)
				{
					result = userManager.Create(user, user.Email);

					if (!result.Succeeded)
					{
						string errors = string.Empty;
						result.Errors.Select(err => errors = string.Format("{0}, {1}", errors, err));
						throw new InvalidOperationException(errors);
					}
				}

				switch (i++)
				{
					case 0:
						{
							userManager.AddToRole(user.Id, roles.ElementAt(2).Name);
							break;
						}
					case 1:
					case 2:
						{
							userManager.AddToRole(user.Id, roles.ElementAt(1).Name);
							break;
						}

					case 3:
						{
							userManager.AddToRole(user.Id, roles.ElementAt(0).Name);
							break;
						}
				}
			}

			#endregion

			#region Statuses
			//this could be used as not tests data 
			List<OrderStatus> statuses = new List<OrderStatus>() 
			{
				new OrderStatus()
				{
					CurrentOrderStatus = Formeo.Models.StaticData.OrderStatusEnum.InProgress
				},

				new OrderStatus()
				{
					CurrentOrderStatus = Formeo.Models.StaticData.OrderStatusEnum.Delivered
				}
			};

			statuses.ForEach(s => context.Statuses.Add(s));

			context.SaveChanges();

			#endregion

			#region Projects
			List<Project> projects = new List<Project>()
			{
				new Project()
				{
					Name = "Order1",
					Creator = users.ElementAt(0),
					Status = statuses.ElementAt(0)
				},
				new Project()
				{
					Name = "Order2",
					Creator = users.ElementAt(0),
					Status = statuses.ElementAt(0)

				},
				new Project()
				{
					Name = "Order3",
					Creator = users.ElementAt(0),
					Status = statuses.ElementAt(0),
				}

			};

			projects.ForEach(o => context.Projects.Add(o));

			context.SaveChanges();

			#endregion

			#region PrintMaterial

			//List<PrintMaterial> printMaterials = new List<PrintMaterial>()
			//{
			//	new PrintMaterial()
			//	{
			//		Name = "PrintMaterial1",
				
			//	},

			//	new PrintMaterial()
			//	{
			//		Name = "PrintMaterial2",
			//	},
			//	new PrintMaterial()
			//	{
			//		Name = "PrintMaterial3",
			//	},
			//	new PrintMaterial()
			//	{
			//		Name = "PrintMaterial4",
			//	},
			//	new PrintMaterial()
			//	{
			//		Name = "PrintMaterial5",
			//	}

			//};

			//printMaterials.ForEach(pm => context.PrintMaterials.Add(pm));

			//context.SaveChanges();

			#endregion

			#region PrintObjects

			List<PrintObject> printObjects = new List<PrintObject>()
			{
				new PrintObject()
				{
					ArticleNo = "1",
					Name = "PrintObject1",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile1",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Steel",
					CompanyCreator = companies.ElementAt(0),
					UserCreator = users.ElementAt(0)
				},

				new PrintObject()
				{
					ArticleNo = "2",
					Name = "PrintObject2",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile2",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Steel",
					CompanyCreator = companies.ElementAt(0),
					UserCreator = users.ElementAt(0)
				},

				new PrintObject()
				{
					ArticleNo = "3",
					Name = "PrintObject3",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile3",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Steel",
					CompanyCreator = companies.ElementAt(0),

				},

				new PrintObject()
				{
					ArticleNo = "4",
					Name = "PrintObject4",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile4",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Wood",
					//Project = projects.ElementAt(2)
				},

				new PrintObject()
				{
					ArticleNo = "5",
					Name = "PrintObject5",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile5",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Wood",
					//Project = projects.ElementAt(2)
				},

				
				new PrintObject()
				{
					Name = "PrintObject6",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile5",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Wood",
					UserCreator = users.ElementAt(0),
					ArticleNo = "123456789",
					CompanyCreator = users.ElementAt(0).Company
				},

				
				new PrintObject()
				{
					Name = "PrintObject7",
					CadFile = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), "mypdf.pdf"),
					PropertiesSpecificationFile = "PropertiesSpecificationFile5",
					CustomerArticleNumber = "CustomerArticleNumber",
					PrintMaterial = "Wood",
					UserCreator = users.ElementAt(0),
					ArticleNo = "99",
					CompanyCreator = users.ElementAt(0).Company
				}
			};

			printObjects.ForEach(po => context.PrintObjects.Add(po));

			context.SaveChanges();

			#endregion

			#region Bids

			//List<Bid> bids = new List<Bid>()
			//{
			//	new Bid() 
			//	{
			//		Price = 10,
			//		PrintObject = printObjects.ElementAt(0), 
			//		Producer = users.ElementAt(1)
			//	},
			//	new Bid ()
			//	{
			//		Price = 20,
			//		PrintObject = printObjects.ElementAt(0), 
			//		Producer = users.ElementAt(2)
			//	},
			//	new Bid 
			//	{
			//		Price = 30,
			//		PrintObject = printObjects.ElementAt(1), 
			//		Producer = users.ElementAt(2)
			//	},
			//	////
			//	new Bid() 
			//	{
			//		Price = 11,
			//		PrintObject = printObjects.ElementAt(1), 

			//		Producer = users.ElementAt(1)
			//	},
			//	new Bid ()
			//	{
			//		Price = 21,
			//		PrintObject = printObjects.ElementAt(2), 
			//		Producer = users.ElementAt(1)
			//	},
			//	new Bid 
			//	{
			//		Price = 31,
			//		PrintObject = printObjects.ElementAt(3), 
			//		Producer = users.ElementAt(2)
			//	}
			//};

			//bids.ForEach(b => context.Bids.Add(b));

			//context.SaveChanges();

			#endregion

			//	projects.ElementAt(0).WinningBid = bids.ElementAt(1);

			context.SaveChanges();



		}
	}
}