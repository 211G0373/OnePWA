using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnePWA.Models.DTOs;
using OnePWA.Services;

namespace OnePWA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsService service;

        public SessionsController(ISessionsService service)
        {
            this.service = service;

        }
        [HttpGet]
        public IActionResult Get()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                var sesion = service.PlayerSession(int.Parse(userId));
                return Ok(sesion);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet]
        [Route("Playing")]
        public IActionResult GetPlaying()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                var sesion = service.PlayingSession(int.Parse(userId));
                return Ok(sesion);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
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

        [HttpPost]
        [Route("joinByCode/{code}")]
        public IActionResult JoinByCode(string code)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            service.JoinSessionByCode(code, int.Parse(userId));

            return Ok();
        }
        [HttpPost]
        [Route("joinRandom")]
        public IActionResult JoinRandom()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            service.JoinRandomSession(int.Parse(userId));

            return Ok();
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> Start()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });

            try
            {
               await service.StartGame(int.Parse(userId));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }

        [HttpPost]
        [Route("PLayCard/{id}")]
        public IActionResult PlayCard(int id)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                service.PlayCard(int.Parse(userId), id);

            }catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPost]
        [Route("BlackCard")]
        public IActionResult BlackCard(ChangeColorDTO dto)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                service.BlackCard(int.Parse(userId), dto);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }


    }
}
