using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnePWA.Models.DTOs;
using OnePWA.Services;

namespace OnePWA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsService service;
        public SessionsController(ISessionsService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult New(CreateSessionDTO dTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)    // "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                   ?? User.FindFirst("sub")?.Value                  // JWT "sub"
                   ?? User.FindFirst("id")?.Value;

            return Ok();
        }


    }
}
