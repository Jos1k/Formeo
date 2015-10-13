using Formeo.BussinessLayer.Interfaces;
using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.BussinessLayer.ManagersImplementation {
	public class PrintObjectsManager : IPrintObjectsManager {

		private ApplicationDbContext _dbContext;
		private ICompaniesManager _comaniesManager;

		public PrintObjectsManager(
			ApplicationDbContext dbContext,
			ICompaniesManager comaniesManager
			) {
			_dbContext = dbContext;
			_comaniesManager = comaniesManager;
		}

		#region  IPrintObjectsManager members
		public IEnumerable<PrintObject> GetPrintObjectsByCreatorCompany( long comapnyId ) {
			List<PrintObject> printObjects =
				_dbContext
					.PrintObjects
					.Where(
						po => po.CompanyCreator.ID == comapnyId )
					.ToList();

			return printObjects;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByIds( IEnumerable<long> printObjectIds ) {
			List<PrintObject> printObjects = _dbContext.PrintObjects.Where(
				po => printObjectIds.Contains( po.ID )
			).ToList();
			return printObjects;
		}

		public PrintObject GetPrintObjectById( long printObjectId ) {
			PrintObject printObject = _dbContext.PrintObjects.Where(
				po => po.ID == printObjectId
			).Single();
			return printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrderId( long orderId ) {
			return from printObject in _dbContext.PrintObjects
				   join relation in _dbContext.ProjectsInfo
				   on printObject.ID equals relation.PrintObjectId
				   where relation.ProjectId == orderId
				   select printObject;
		}

		public IEnumerable<PrintObject> GetNeedBidPrintObjectsForProducer( string producerId, bool isNeedBid ) {

			Company producerCompany = _comaniesManager.GetCompanyByUserId( producerId );

			IEnumerable<PrintObject> printObjectsResult =
				from printObject in _dbContext.PrintObjects
				where
					!( from bid in _dbContext.Bids
					   where bid.CompanyProducer.ID == producerCompany.ID
					   select bid.PrintObject.ID )
					.Contains( printObject.ID )
					&& printObject.IsNeedBid == isNeedBid
				select printObject;

			return printObjectsResult;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByCompanyProducer(
				long companyId,
				Formeo.Models.StaticData.PrintObjectStatusEnum poStatus
			) {
			return from printObject in _dbContext.PrintObjects
				   join projectInfo in _dbContext.ProjectsInfo
				   on printObject.ID equals projectInfo.PrintObject.ID
				   where projectInfo.CompanyProducer.ID == companyId
				   select printObject;
		}

		public IEnumerable<PrintObject> GetPrintObjectsByOrder( long orderId ) {
			return _dbContext.PrintObjects.Join(
					_dbContext.ProjectsInfo,
					po => po.ID,
					pi => pi.PrintObjectId,
					( po, pi ) => po
				);
		}

		public bool ToggleIsNeedBid( long printObjectId ) {
			PrintObject printObject = _dbContext.PrintObjects.Where( po => po.ID == printObjectId ).SingleOrDefault();
			if( printObject == null ) {
				throw new InvalidOperationException( "printObjectId is Invalid" );
			}
			printObject.IsNeedBid = !printObject.IsNeedBid;
			_dbContext.SaveChanges();

			return printObject.IsNeedBid;
		}

		public PrintObject UploadPrintObject( string userId, string articleNo, string productName, string pathToFile, int printMaterialId ) {

			ApplicationUser creator = _dbContext.Users.FirstOrDefault( x => x.Id == userId );
			PrintObject resultPrintObject = new PrintObject() {
				IsNeedBid = false,
				ArticleNo = Convert.ToInt64( articleNo ),
				UserCreator = creator,
				CompanyCreator = creator.Company,
				Name = productName,
				PrintMaterial = _dbContext.PrintMaterials.FirstOrDefault( x => x.ID == printMaterialId ),
				CadFile = pathToFile
			};
			_dbContext.PrintObjects.Add( resultPrintObject );
			_dbContext.SaveChanges();

			return resultPrintObject;
		}
		#endregion
		#region IPrintObjectsManager Members
		#endregion
	}
}