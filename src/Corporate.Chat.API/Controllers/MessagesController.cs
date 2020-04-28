using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Corporate.Chat.API.Context;
using Corporate.Chat.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Corporate.Chat.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatContext context;

        public MessagesController(ChatContext context)
        {
            this.context = context;
        }

        // GET api/messages
        [HttpGet("All")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Message>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await context.Messages.AsNoTracking().ToListAsync());
        }

        // GET api/messages?page={0}&pageSize={1}
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PagedResult<Message>))]
        public async Task<IActionResult> GetPaged([FromQuery] int page, int pageSize)
        {
            return Ok(await context.Messages.AsNoTracking().GetPagedAsync(page, pageSize));
        }
    }
}