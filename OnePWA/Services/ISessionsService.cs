using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePWA.Models;
using OnePWA.Models.DTOs;

namespace OnePWA.Services
{
    public interface ISessionsService
    {
        IGameService GameService { get; }
        string CreateSession(ICreateSesionDTO sesionDTO, int idHost);
        ISesionDTO JoinSessionByCode(string code,int id);
        ISesionDTO JoinRandomSession(int id);
        ISesionDTO PlayAgain(int id);
        void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove);
        void LeaveSession(int idPlayer);
    }
}
