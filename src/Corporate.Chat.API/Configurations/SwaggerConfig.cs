using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;


namespace Corporate.Chat.API.Configurations
{
	/// <summary>
	/// 
	/// </summary>
	public class SwaggerConfig
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Info GetSwashbuckleApiInfo()
		{
			return new Info
			{
				Title = "Corporate Chat API",
				Description = "Chat API docs.",
				Version = GetVersion(),
				Contact = new Contact
				{
					Email = "viniciusduartereis@icloud.com",
					Name = "Vinicius Duarte Reis",
					Url = ""
				}
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string GetVersion()
		{
			string version = PlatformServices.Default.Application.ApplicationVersion;

			version = "v" + version.Substring(0, version.IndexOf('.'));

			return version;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string GetApplicationName()
		{
			var applicationName = PlatformServices.Default.Application.ApplicationName;

			if (applicationName.ToLower().EndsWith(".api"))
			{
				applicationName = applicationName.Remove(applicationName.Length - 4, 4);
			}

			return applicationName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string GetApiXmlCommentsPath()
		{
			ApplicationEnvironment application = PlatformServices.Default.Application;
			return Path.Combine(application.ApplicationBasePath, string.Format("{0}.xml", (object) application.ApplicationName));
		}

	
	}
}