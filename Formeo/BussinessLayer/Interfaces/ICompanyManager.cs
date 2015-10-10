﻿using Formeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formeo.BussinessLayer.Interfaces
{
	public interface ICompaniesManager
	{
		Company GetCompanyByUserId(string userId);
	}
}
