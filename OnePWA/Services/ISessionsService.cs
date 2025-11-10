using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePWA.Models;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public interface ISessionsService
    {
        IRepository<Cards> Repository { get; }
        IGameService GameService { get; }
        ISesionDTO CreateSession(ICreateSesionDTO sesionDTO, int idHost);
        ISesionDTO JoinSessionByCode(string code,int id);
        ISesionDTO JoinRandomSession(int id);
        ISesionDTO PlayAgain(int id);
        void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove);
        void LeaveSession(int idPlayer);
    }
}
