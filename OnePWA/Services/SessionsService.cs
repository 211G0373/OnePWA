
using OnePWA.Models;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public class SessionsService : ISessionsService
    {
      

        public IRepository<Users> usersRepository { get; }
        public ISessionsRepository sessionsRepository { get; }

        public SignalrService signalrService { get; }
        public SessionsService(IRepository<Users> repository, ISessionsRepository sessionsRepository, SignalrService signalrService)
        {
            usersRepository = repository;
            this.sessionsRepository = sessionsRepository;
            this.signalrService = signalrService;
        }

        public ISessionDTO PlayerSession(int id)
        {
            var session = sessionsRepository.GetByPlayerId(id);
            if (session == null)
            {
                throw new Exception("Player is not in a session");
            }
            return new SessionDTO
            {
                Name = session.Name,
                Code = session.Code,
                IdHost = session.IdHost,
                Players = session.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    UserName = usersRepository.Get(p.Id).Name
                }).ToList()
            };
        }

        public bool CreateSession(ICreateSesionDTO sesionDTO, int idHost)
        {
            GameSession newSession = new GameSession
            {
                Name = sesionDTO.Name,
                Private = sesionDTO.Private,
                NewRules = sesionDTO.NewRules,
                IdHost = idHost,
            };
            Player hostPlayer = new Player
            {
                Id = idHost
            };
            newSession.Code = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            newSession.Players.Add(hostPlayer);

            sessionsRepository.Insert(newSession);
            return true;

        }

        public void JoinRandomSession(int id)
        {
            throw new NotImplementedException();
        }

        public bool JoinSessionByCode(string code, int id)
        {
            if(PlayerSession(id) != null)
            {
                throw new Exception("Player already in a session");
            }

            var session = sessionsRepository.GetByCode(code);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            Player newPlayer = new Player
            {
                Id = id
            };



            session.Players.Add(newPlayer);

            foreach(var player in session.Players)
            {
                if (player.Id != id)
                {
                    var playerDTO = new PlayerDTO
                    {
                        Id = newPlayer.Id,
                        UserName = usersRepository.Get(newPlayer.Id).Name
                    };
                    signalrService.PlayerJoined(player.Id.ToString(), playerDTO);
                }
            }


            return true;


        }

        public void LeaveSession(int idPlayer)
        {
            throw new NotImplementedException();
        }

        public void PlayAgain(int id)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove)
        {
            throw new NotImplementedException();
        }
    }
}
