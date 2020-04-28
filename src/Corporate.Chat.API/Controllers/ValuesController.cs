using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.Chat.API.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{

		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { 
			 Environment.GetEnvironmentVariable("ASPNETCORE_HOSTNAME") };
		}
	}
}
