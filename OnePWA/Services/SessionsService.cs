
using OnePWA.Models;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public class SessionsService : ISessionsService
    {
        public IRepository<Cards> Repository { get; }

        public IGameService GameService { get; }

        public SessionsService( IGameService gameService)
        {
         //  Repository = repository;
            GameService = gameService;
        }

        public ISesionDTO CreateSession(ICreateSesionDTO sesionDTO, int idHost)
        {
              GameSession newSession = new GameSession
              {
                  Name = sesionDTO.Name,
                  IsPublic = !sesionDTO.Private,
                  Started = false,
                  IdHost = idHost,
              };
              
             newSession.Players
        }

        public ISesionDTO JoinRandomSession(int id)
        {
            throw new NotImplementedException();
        }

        public ISesionDTO JoinSessionByCode(string code, int id)
        {
            throw new NotImplementedException();
        }

        public void LeaveSession(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public ISesionDTO PlayAgain(int id)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove)
        {
            throw new NotImplementedException();
        }
    }
}
