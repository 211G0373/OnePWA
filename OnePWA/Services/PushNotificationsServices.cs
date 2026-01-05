using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;
using System.Text.Json;
using WebPush;

namespace OnePWA.Services
{
    public class PushNotificationsServices: IPushNotificationServices
    {
        VapidDetails vapid;

        public PushNotificationsServices(IRepository<PushSusbcrption> repository, IConfiguration configuration)
        {
            Repository=repository;
            Configuration=configuration;
            vapid = new VapidDetails
            {
                PrivateKey = configuration["VAPID:privateKey"],
                PublicKey = configuration["VAPID:publicKey"],
                Subject = configuration["VAPID:subject"],
            };
        }

        public IRepository<PushSusbcrption> Repository { get; }
        public IConfiguration Configuration { get; }
        public void Suscribir(SubscriptionDTO dto)
        {
            var entidad = Repository.GetAll().FirstOrDefault(x => x.Endpoint == dto.Endpoint);

            if (entidad == null)
            {
                entidad = new PushSusbcrption
                {
                    Endpoint = dto.Endpoint,
                    Auth = dto.Keys.Auth,
                    P256dh = dto.Keys.P256dh,
                    Activo = true,
                    FechaCreacion = DateTime.Now,

                };
                Repository.Insert(entidad);
            }
        }

        public void Desuscribir(string endpoint)
        {
            var entidad = Repository.GetAll().FirstOrDefault(x => x.Endpoint == endpoint);
            if (entidad != null)
            {
                Repository.Delete(entidad);
            }
        }

        public string GetPublicKey()
        {
            return vapid.PublicKey;
        }

        public async Task EnviarMensaje(string titulo, string mensaje)
        {
            var destinatarios = Repository.GetAll().Where(x => x.Activo == true).ToList();


            foreach (var d in destinatarios)
            {
                try
                {
                    var cliente = new WebPushClient();
                    PushSubscription quien = new PushSubscription(d.Endpoint, d.P256dh, d.Auth);

                    var message = new
                    {
                        titulo = titulo,
                        mensaje = mensaje
                    };

                    ; await cliente.SendNotificationAsync(quien,
                        JsonSerializer.Serialize(message),
                        vapid);
                    d.FechaUltimaNotificacion = DateTime.Now;
                    Repository.Update(d);
                }
                catch (WebPushException ex)
                {
                    if (ex.StatusCode == System.Net.HttpStatusCode.Gone)
                    {
                        Repository.Delete(d.Id);
                    }
                }

            }
        }
    }
}
