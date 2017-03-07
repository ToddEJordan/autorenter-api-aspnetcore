using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Features.Logs
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IMediator _mediator;

        public LogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Post.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mediator.Send(command);

            return StatusCode(201);
        }
    }
}