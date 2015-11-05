using System.Threading.Tasks;
using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using Formeo.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation
{
	public class PrintObjectsManager : IPrintObjectsManager
	{

		private ApplicationDbContext _dbContext;
		private ICompaniesManager _comaniesManager;
		private IUserManager _userManager;

		public PrintObjectsManager(
			ApplicationDbContext dbContext,
			ICompaniesManager comaniesManager,
			IUserManager userManager
			)
		{
			_dbContext = dbContext;
			_comaniesManager = comaniesManager;
			_userManager = userManager;
		}

		#region  IPrintObjectsManager members
		public IEnumerable<PrintObject> GetPrintObjectsByCreatorCompany(long comapnyId)
		{

			List<PrintObject> printObjects =
				_dbContext
					.PrintObjects
					.Include("CompanyProducer")
					.Where(
						po => po.CompanyCreator.ID == comapnyId)
					.ToList();

			return printObjects;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds)
		{
			List<PrintObject> printObjects = _dbContext
				.PrintObjects
				.Include("Bids")
				.Include("CompanyProducer")
				.Where(
					po => printObjectIds.Contains(po.ID)
				).ToList();

			return printObjects;
		}

		public PrintObject GetPrintObjectById(long printObjectId)
		{
			PrintObject printObject = _dbContext
				.PrintObjects
				.Include("CompanyProducer")
				.Where(
				po => po.ID == printObjectId
			).Single();
			return printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectByOrderId(long orderId)
		{
			return from printObject in _dbContext.PrintObjects
				   join projectInfo in _dbContext.ProjectInfos
				   on printObject.ID equals projectInfo.PrintObjectId
				   where projectInfo.ProjectId == orderId
				   select printObject;
		}

		public IEnumerable<PrintObject> GetNeedBidPrintObjectsForProducer(string producerId, bool isNeedBid)
		{

			Company producerCompany = _comaniesManager.GetCompanyByUserId(producerId);

			IEnumerable<PrintObject> printObjectsResult =
				from printObject in _dbContext.PrintObjects
				where
					!(from bid in _dbContext.Bids
					  where bid.CompanyProducer.ID == producerCompany.ID
					  select bid.PrintObject.ID)
					.Contains(printObject.ID)
					&& printObject.IsNeedBid == isNeedBid
				select printObject;

			return printObjectsResult;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrder(long orderId)
		{
			return _dbContext.PrintObjects.Join(
					_dbContext.ProjectInfos,
					po => po.ID,
					pi => pi.PrintObjectId,
					(po, pi) => po
				);
		}

		public bool ToggleIsNeedBid(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects
				.SingleOrDefault(po => po.ID == printObjectId);
			if (printObject == null)
			{
				throw new InvalidOperationException("printObjectId is Invalid");
			}
			printObject.IsNeedBid = !printObject.IsNeedBid;
			_dbContext.SaveChanges();
			_dbContext.Entry(printObject).Reload();


			if( printObject.IsNeedBid ) { 
			List<string> producersEmail = _userManager
				.GetUsersByRole(StaticData.RoleNames.Producer)
				.Select(producer=>producer.Email)
				.ToList();
			
			foreach(string producerEmail in producersEmail) {
				StaticData.SendEmail(
					producerEmail,
					"You have a bid request", 
					string.Format(
						"Hello!\n\nYou have a new bid request in Formeo from {0} for {1}",
						printObject.UserCreator.UserName,
						printObject.ArticleNo
					)
				);
			}
			}


			return printObject.IsNeedBid;
		}

		public IEnumerable<PrintObject> UploadPrintObjects(IEnumerable<PrintObjectFileInfo> fileInfos)
		{

			if (fileInfos == null || fileInfos.Count() == 0)
			{
				return null;
			}


			IList<PrintObject> resultPrintObjects = new List<PrintObject>();
			foreach (PrintObjectFileInfo fileInfo in fileInfos)
			{
				string currentCompanyName = _comaniesManager.GetCurrentCompany().Name;
				string dirName = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/uploads"), currentCompanyName);
				Directory.CreateDirectory(dirName);

				dirName = Path.Combine(dirName, Guid.NewGuid().ToString());
				Directory.CreateDirectory(dirName);

				string fileName = Path.Combine(dirName, fileInfo.File.FileName);
				fileInfo.File.SaveAs(fileName);

				resultPrintObjects.Add(CreatePrintObjectByFileInfo(fileInfo, fileName));
			}
			return resultPrintObjects;
		}

		public PrintObject AssignProducerToPrintObject(long producerCompanyId, long printObjectId)
		{
			PrintObject printObject = GetPrintObjectById(printObjectId);
			Company producerCompany = _comaniesManager.GetCompanyById(producerCompanyId);

			if (printObject.CompanyProducer == null ||
				(printObject.CompanyProducer.ID != producerCompany.ID)) //saving the same data causes errors...and it's bad for karma 
			{
				printObject.CompanyProducer = producerCompany;
				_dbContext.SaveChanges();
				_dbContext.Entry(printObject).Reload();
			}

			return printObject;
		}

		public IEnumerable<PrintObject> GetExclusivePrintObjectsByIdsForCompanyJSON(long companyId, IEnumerable<long> printObjectIdsToExlude) 
		{
			return _dbContext
				.PrintObjects
				.Include("CompanyProducer")
				.Where(printObject => printObject.CompanyCreator.ID == companyId
						&& printObject.CompanyProducer != null
						&& !printObjectIdsToExlude.Contains(printObject.ID));
		}
		#endregion

		private PrintObject CreatePrintObjectByFileInfo(PrintObjectFileInfo fileInfo, string fileName)
		{
			ApplicationUser creator = _userManager.GetCurrentUser();
			PrintObject resultPrintObject = new PrintObject()
			{
				IsNeedBid = false,
				ArticleNo = fileInfo.ArtNo,
				UserCreator = creator,
				CompanyCreator = creator.Company,
				Name = fileInfo.ProductName,
				PrintMaterial = fileInfo.PrintMaterial,
				CadFile = fileName
			};
			_dbContext.PrintObjects.Add(resultPrintObject);
			_dbContext.SaveChanges();

			return resultPrintObject;
		}

		public string GetPrintObjectFilePath(long printObjectId)
		{
			return _dbContext.PrintObjects.Find(printObjectId).CadFile;
		}

	}
}