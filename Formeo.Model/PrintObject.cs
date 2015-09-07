using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Model
{
	public class PrintObject
	{
		[Key]
		[ForeignKey("OrderLine")]
		public long ID { get; set; }
		public string Name { get; set; }

		public string CadFile { get; set; }

		public string PropertiesSpecificationFile { get; set; }

		public string CustomerArticleNumber { get; set; }
		public double CubicInches { get; set; }

		public virtual OrderLine OrderLine { get; set; }

		public virtual PrintMaterial PrintMaterial { get; set; }
	}
}
