using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	public class ProjectInfo
	{
		[Key, ForeignKey("Project"), Column(Order = 0)]
		public virtual long ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[Key, ForeignKey("PrintObject"), Column(Order = 1)]
		public virtual long PrintObjectId { get; set; }
		public virtual PrintObject PrintObject { get; set; }

		public virtual Company CompanyProducer { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public Formeo.Models.StaticData.PrintObjectStatusEnum Status { get; set; }
	}
}