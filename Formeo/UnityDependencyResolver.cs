using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Formeo
{
	class UnityDependencyResolver : IDependencyResolver
	{
		UnityContainer _unityContainer;
		public UnityDependencyResolver(UnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}
		public object GetService(Type serviceType)
		{
			try
			{
				return _unityContainer.Resolve(serviceType);
			}
			catch {
				return null;
			}
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return _unityContainer.ResolveAll(serviceType);
			}
			catch
			{
				return new List<object>();
			}
		}
	}
}
