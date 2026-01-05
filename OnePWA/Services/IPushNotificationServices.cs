using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public interface IPushNotificationServices
    {
        IRepository<PushSusbcrption> Repository { get; }
        void Suscribir(SubscriptionDTO dto);
        void Desuscribir(string endpoint);
        string GetPublicKey();
        Task EnviarMensaje(string titulo, string mensaje);


    }
}
