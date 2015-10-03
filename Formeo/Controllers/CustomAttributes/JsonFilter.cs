﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Formeo.Controllers.CustomAttributes
{

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class JsonQueryParamFilter : ActionFilterAttribute
	{
		public string Param { get; set; }
		public Type JsonDataType { get; set; }
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Request.ContentType.Contains("application/json"))
			{
				string inputContent = filterContext.HttpContext.Request.QueryString[Param];

				var result = JsonConvert.DeserializeObject(inputContent, JsonDataType);
				filterContext.ActionParameters[Param] = result;
			}
		}
	}
}