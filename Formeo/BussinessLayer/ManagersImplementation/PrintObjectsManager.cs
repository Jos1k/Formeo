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
					.Where(
						po => po.CompanyCreator.ID == comapnyId)
					.ToList();

			return printObjects;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByIds(IEnumerable<long> printObjectIds)
		{
			List<PrintObject> printObjects = _dbContext.PrintObjects.Where(
				po => printObjectIds.Contains(po.ID)
			).ToList();
			return printObjects;
		}

		public PrintObject GetPrintObjectById(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(
				po => po.ID == printObjectId
			).Single();
			return printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrderId(long orderId)
		{
			return from printObject in _dbContext.PrintObjects
				   join relation in _dbContext.ProjectsInfo
				   on printObject.ID equals relation.PrintObjectId
				   where relation.ProjectId == orderId
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

		public IEnumerable<PrintObject> GetPrintObjectsByCompanyProducer
		(
			long companyId,
			Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
		)
		{
			return _dbContext.PrintObjects
				.Where(po => po.CompanyProducer.ID == companyId)
				.Join(
					_dbContext.ProjectsInfo
					, printObject => printObject.ID
					, projectInfo => projectInfo.PrintObjectId
					, (printObject, projectInfo) => printObject
				)
				.ToArray();
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrder(long orderId)
		{
			return _dbContext.PrintObjects.Join(
					_dbContext.ProjectsInfo,
					po => po.ID,
					pi => pi.PrintObjectId,
					(po, pi) => po
				);
		}

		public bool ToggleIsNeedBid(long printObjectId)
		{
			PrintObject printObject = _dbContext.PrintObjects.Where(po => po.ID == printObjectId).SingleOrDefault();
			if (printObject == null)
			{
				throw new InvalidOperationException("printObjectId is Invalid");
			}
			printObject.IsNeedBid = !printObject.IsNeedBid;
			_dbContext.SaveChanges();
			_dbContext.Entry(printObject).Reload();
			return printObject.IsNeedBid;
		}

		//public void AssignProducerToPrintObject(long producerCompanyId, long printObjectId)
		//{
		//	PrintObject printObject = GetPrintObjectById(printObjectId);
		//	Company producerCompany = _comaniesManager.GetCompanyById(producerCompanyId);

		//	printObject.CompanyProducer = producerCompany;
		//	_dbContext.SaveChanges();
		//}

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
				Directory.CreateDirectory( dirName );

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
				(printObject.CompanyProducer.ID != producerCompany.ID)) //saving the same data causes errors...at it's bad for karma 
			{
				printObject.CompanyProducer = producerCompany;
				_dbContext.SaveChanges();
				_dbContext.Entry(printObject).Reload();
			}

			return printObject;
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

		public string GetPrintObjectFilePath( long printObjectId ) {
			return _dbContext.PrintObjects.Find( printObjectId ).CadFile;
		}

		#region IPrintObjectsManager Members

		#endregion
	}
}