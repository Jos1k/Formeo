using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.Models
{
	public class PrintObject
	{
		[Key]
		public long ID { get; set; }
		public string Name { get; set; }

		public virtual ApplicationUser UserCreator { get; set; }

		public virtual Company CompanyCreator { get; set; }
		public Company CompanyProducer { get; set; }

		public long ArticleNo { get; set; }

		public string CadFile { get; set; }

		public string PropertiesSpecificationFile { get; set; }

		public string CustomerArticleNumber { get; set; }
		public double CubicInches { get; set; }

		public virtual ICollection<ProjectInfo> ProjectPrintObjectQuantityRelatios { get; set; }

		public virtual ICollection<Bid> Bids { get; set; }

		public virtual string PrintMaterial { get; set; }
		public bool IsNeedBid { get; set; }
	}
}
