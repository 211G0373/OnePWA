using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace OnePWA.Providers
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Usa el claim "sub" o "nameid" del JWT
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? connection.User?.FindFirst("sub")?.Value;
        }
    }
}
