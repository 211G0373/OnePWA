using Microsoft.AspNetCore.SignalR;

namespace OnePWA.Services
{
    public class SignalrService: Hub
    {
        //JugadorTomoCarta, se manda a todos los jugadores menos a quien la tomo
        //

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Usuario conectado: {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }

        public async Task EnviarMensajeAUsuario(string userId, string mensaje)
        {
            await Clients.User(userId).SendAsync("RecibirMensaje", mensaje);
        }

    }
}
