using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	public class ProjectPrintObjectQuantityRelation
	{
		[Key, ForeignKey("Project"), Column(Order=0)]
		public virtual long ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[Key, ForeignKey("PrintObject"), Column(Order = 1)]
		public virtual long PrintObjectId { get; set; }
		public virtual PrintObject PrintObject { get; set; }
		public long Quantity { get; set; }
	}
}