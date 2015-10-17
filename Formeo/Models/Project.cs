using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class Project
	{
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }

		public virtual ApplicationUser Creator { get; set; }

		public virtual Company CompanyCreator { get; set; }

		public virtual Formeo.Models.StaticData.OrderStatusEnum Status { get; set; }

		public virtual ICollection<ProjectInfo> ProjectPrintObjectQuantityRelations { get; set; }

		public decimal OrderPrice { get; set; }

		[NotMapped]
		//quantity of all printobjects in this project
		public int OverallQuantity { get { return GetOverallQuantity(); } }


		#region DeliveryInfo

		public string Surname { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }

		public string ZipCode { get; set; }

		public string City { get; set; }
		public string Country { get; set; }

		#endregion

		private int GetOverallQuantity()
		{
			if (ProjectPrintObjectQuantityRelations == null
				|| ProjectPrintObjectQuantityRelations.Count == 0)
			{
				return 0;
			}
			int overralQuantity = 0;

			foreach (var relation in ProjectPrintObjectQuantityRelations)
			{
				overralQuantity += relation.Quantity;
			}

			return overralQuantity;
		}
	}
}
