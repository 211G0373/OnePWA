using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePWA.Models;
using OnePWA.Models.DTOs;

namespace OnePWA.Services
{
    public interface ISesionsService
    {
        IEnumerable<IGameSesion> Sesions { get; set; }

        string CreateSesion(ICreateSesionDTO sesionDTO, int idHost);

        ISesionDTO JoinSesionByCode(string code,int id);

        ISesionDTO GetSesionByCode();




    }
}
