using MediatR;
using MediSearch.Application.Features.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediSearch.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authentication : ControllerBase
    {
        readonly IMediator _mediator;
        readonly ILogger<Authentication> _logger;
        public Authentication(IMediator mediator, ILogger<Authentication> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthRequest request)
        {
            var responce = await _mediator.Send(request);
            if (responce == null)
            {
                return BadRequest("Enter valid username or password");
            }
            return Ok(responce.Token);
        }



    }
}
