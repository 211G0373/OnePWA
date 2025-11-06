using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePWA.Models;
using OnePWA.Models.DTOs;

namespace OnePWA.Services
{
    public interface ISesionsService
    {
        IGameService GameService { get; }
        string CreateSesion(ICreateSesionDTO sesionDTO, int idHost);

        ISesionDTO JoinSesionByCode(string code,int id);

        ISesionDTO JoinRandomSesion(int id);

        ISesionDTO PlayAgain(int id);

        void RemovePlayerFromSesion(int idPlayer, int idPlayerForRemove);

        void LeaveSesion(int idPlayer);


    }
}
