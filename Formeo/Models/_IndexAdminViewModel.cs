using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formeo.Models
{
	public class _IndexAdminViewModel
	{
		public List<ApplicationUser> Customers { get; set; }
		public List<ApplicationUser> Producers { get; set; }
	}
}