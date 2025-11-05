using AutoMapper;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public interface ICardsService
    {
        IMapper Mapper { get; }

        IRepository<Cards> Repository { get; }
        IEnumerable<ICardDTO> GetAllCards();
        Cards? GetCardById(int id);

      

    }
}
