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


        [HttpPost]
        [Route("Replay")]
        public IActionResult Replay()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                service.Replay(int.Parse(userId));
                return Ok();
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
        [Route("RemovePlayer/{id}")]
        public IActionResult RemovePlayer(int id)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                service.RemovePlayerFromSession(int.Parse(userId), id);
                return Ok();
            }
            catch
            {
                return NotFound();

            }






        }

        [HttpPost]
        public async Task<IActionResult> New(CreateSessionDTO dto)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });

            var session = await service.CreateSession(dto, int.Parse(userId));
            return Ok(session);
        }

        [HttpPost]
        [Route("joinByCode/{code}")]
        public async Task<IActionResult> JoinByCode(string code)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            await service.JoinSessionByCode(code, int.Parse(userId));

            return Ok();
        }
        [HttpPost]
        [Route("joinRandom")]
        public async Task<IActionResult> JoinRandom()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });

            try
            {
                await service.JoinRandomSession(int.Parse(userId));
            }
            catch {
                return NotFound();
            
            }

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
        [Route("leave")]
        public async Task<IActionResult> Abandonar()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });

            try
            {
                service.LeaveSession(int.Parse(userId));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }






        [HttpPost]
        [Route("PLayCard/{id}")]
        public async Task<IActionResult> PlayCard(int id)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                await service.PlayCard(int.Parse(userId), id);

            }catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }


        [HttpPost]
        [Route("BlackCard")]
        public async Task<IActionResult> BlackCard(ChangeColorDTO dto)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "No se encontró el ID del usuario en el token." });
            try
            {
                await service.BlackCard(int.Parse(userId), dto);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            return Ok();
        }


    }
}
