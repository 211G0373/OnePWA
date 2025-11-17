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

        public IEnumerable<CardDTO> GetAllCardsDTOs()
        {
            return Repository.GetAll().Select(card => new CardDTO() { Id = card.Id, Name = card.Name});
        }

        public Cards? GetCardById(int id)
        {
            return Repository.Get(id);
        }
    }
}
