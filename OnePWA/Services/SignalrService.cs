using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using OnePWA.Models.DTOs;

namespace OnePWA.Services
{
    public class SignalrService: Hub
    {
        //JugadorTomoCarta, se manda a todos los jugadores menos a quien la tomo
        //

        public static ConcurrentDictionary<string, string> Users = new();

        public override Task OnConnectedAsync()
        {
            // Obtén el ID del usuario autenticado
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? Context.User?.FindFirst("id")?.Value;

            if (userId != null)
                Users[userId] = Context.ConnectionId;
          
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //eliminar del diccionario de usuarios conectados




            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? Context.User?.FindFirst("Id")?.Value
                         ?? Context.User?.FindFirst("id")?.Value;

            if (userId != null && Users.ContainsKey(userId))
            {
                Users.TryRemove(userId, out _);
                Console.WriteLine($"🧩 Usuario {userId} desconectado.");
            }
            else
            {
                Console.WriteLine("⚠️ OnDisconnectedAsync: userId null o no encontrado en Users.");
            }

            await base.OnDisconnectedAsync(exception);
        }


        //carta movida
        public async Task PlayerColocoCard(string targetUserId, IMovementDTO dTO)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("PlayerColocoCard", dTO);
        }

        public async Task PlayerTakeCard(string targetUserId, TakedCard dto)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("PlayerTakeCard", dto);
        }

        public async Task YouTakeCard(string targetUserId,CardTaked dto)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("YouTakeCard", dto);
        }




        public async Task PlayerJoined(string targetUserId, IPlayerDTO dTO)
        {



            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("PlayerJoined", dTO);
        }

        public async Task PlayerLeft(string targetUserId, int idplayer)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("PlayerLeft",idplayer);
        }


        public async Task GameStarted(string targetUserId)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("GameStarted");
        }



        // Enviar mensaje a un usuario específico
        public async Task SendToUser(string targetUserId, string message)
        {
            if (Users.TryGetValue(targetUserId, out var connId))
                await Clients.Client(connId).SendAsync("ReceiveMessage", message);
        }

        // Enviar mensaje a todos
        public async Task SendToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Enviar mensaje a un grupo
        public async Task SendToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        // Agregar usuario a un grupo
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

    }
}
