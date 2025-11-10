using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult New(CreateSessionDTO dto)
        { 
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });

            var session = service.CreateSession(dto, int.Parse(userId));

            return Ok(session);
        }


    }
}
