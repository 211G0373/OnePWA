using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnePWA.Models.DTOs;
using OnePWA.Services;

namespace OnePWA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService service;

        public UsersController(IUsersService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(SignUpDTO dto)
        {
            service.SignUp(dto);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var token = service.Login(dto);
            if (token == string.Empty)
            {
                return BadRequest("Correo electronico o contraseña incorrecta");
            }
            return Ok(token);
        }
    }
}
