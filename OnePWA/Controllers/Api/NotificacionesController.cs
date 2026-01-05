using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnePWA.Models.DTOs;
using OnePWA.Services;
using System.Security.Cryptography.X509Certificates;

namespace OnePWA.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        public NotificacionesController(IPushNotificationServices service)
        {
            Service=service;
        }

        public IPushNotificationServices Service { get; }

        [HttpGet("publickey")]
        public IActionResult GetPublicKey()
        {
            return Ok(Service.GetPublicKey());
        }

        [HttpPost]
        public IActionResult Post(SubscriptionDTO dto)
        {

            Service.Suscribir(dto);
            return Ok();
        }
    }
}
