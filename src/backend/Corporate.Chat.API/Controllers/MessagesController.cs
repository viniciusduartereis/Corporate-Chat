using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Model;
using Corporate.Chat.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Corporate.Chat.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageAppService service;

        public MessagesController(IMessageAppService service)
        {
            this.service = service;
        }

        // GET api/messages
        [HttpGet("All")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Message>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await service.GetAllAsync(cancellationToken));
        }

        // GET api/messages?page={0}&pageSize={1}
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PagedResult<Message>))]
        public async Task<IActionResult> GetPaged([FromQuery] int page, int pageSize, CancellationToken cancellationToken)
        {
            return Ok(await service.GetPagedAsync(page, pageSize, cancellationToken));
        }
    }
}