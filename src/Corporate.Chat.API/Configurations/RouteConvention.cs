using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Corporate.Chat.API.Configurations
{
	/// <summary>
	/// 
	/// </summary>
	public class RouteConvention : IApplicationModelConvention
	{
		private readonly AttributeRouteModel _centralPrefix;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="routeTemplateProvider"></param>
		public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
		{
			_centralPrefix = new AttributeRouteModel(routeTemplateProvider);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="application"></param>
		public void Apply(ApplicationModel application)
		{
			foreach (var controller in application.Controllers)
			{
				var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
				if (matchedSelectors.Any())
				{
					foreach (var selectorModel in matchedSelectors)
					{
						selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
							selectorModel.AttributeRouteModel);
					}
				}

				var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();

				if (unmatchedSelectors.Any())
				{
					foreach (var selectorModel in unmatchedSelectors)
					{
						selectorModel.AttributeRouteModel = _centralPrefix;
					}
				}
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class MvcOptionsExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="opts"></param>
		/// <param name="routeAttribute"></param>
		public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
		{
			opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
		}
	}
}
