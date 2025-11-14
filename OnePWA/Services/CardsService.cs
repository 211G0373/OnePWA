using AutoMapper;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public class CardsService : ICardsService
    {
        public IMapper Mapper { get; set; }

        public IRepository<Cards> Repository { get; set; }

        public CardsService(IRepository<Cards> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public IEnumerable<Cards> GetAll()
        {
            return Repository.GetAll();
        }

        public IEnumerable<ICardDTO> GetAllCardsDTOs()
        {
            throw new NotImplementedException();
        }

        public Cards? GetCardById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
