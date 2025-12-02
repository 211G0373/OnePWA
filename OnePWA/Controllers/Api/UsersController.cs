using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Services;

namespace OnePWA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService service;
        private readonly EmailService emailService;

        public OnecgdbContext Context { get; }

        public UsersController(IUsersService service, EmailService emailService, OnecgdbContext context)
        {
            this.service = service;
            this.emailService=emailService;
            Context=context;
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


        [HttpPost("restablecer")]
        public async Task<IActionResult>? Restablecer(string correo)
        {
            var email = Context.Users.FirstOrDefault(x => x.Email == correo);

            if (email==null)
            {
                return BadRequest("Correo electronico invalido");

            }
            else
            {
                emailService.SendEmailAsync(correo);
            }

                return Ok();
        }
    }
}
