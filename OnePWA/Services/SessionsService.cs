
using OnePWA.Models;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;
using System.Threading.Tasks;

namespace OnePWA.Services
{
    public class SessionsService : ISessionsService
    {
      

        public IRepository<Users> usersRepository { get; }
        public ISessionsRepository sessionsRepository { get; }

        public SignalrService signalrService { get; }
        public ICardsService Cards { get; }

        public SessionsService(IRepository<Users> repository, ISessionsRepository sessionsRepository, SignalrService signalrService, ICardsService cards)
        {
            usersRepository = repository;
            this.sessionsRepository = sessionsRepository;
            this.signalrService = signalrService;
            Cards = cards;
        }


        public IWaittingSessionDTO PlayerSession(int id)
        {
            var session = sessionsRepository.GetByPlayerId(id);
            if (session == null)
            {
                throw new Exception("Player is not in a session");
            }
            return new WaittingSessionDTO
            {
                Name = session.Name,
                Code = session.Code,
                IdHost = session.IdHost,
                Players = session.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    UserName = usersRepository.Get(p.Id).Name,
                    CardsCount = p.Cards.Count()
                }).ToList()
            };
        }
        public IPlayingSessionDTO PlayingSession(int id)
        {
            var session = sessionsRepository.GetByPlayerId(id);
            if (session == null)
            {
                throw new Exception("Player is not in a session");
            }
            var player = session.Players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                throw new Exception("Player not found in session");
            }
            return new PlayingSessionDTO
            {
                Name = session.Name,
                PlayerCount = session.Players.Count(),
                IdTurn = session.IdTurn,
                Players = session.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    UserName = usersRepository.Get(p.Id).Name,
                    CardsCount = p.Cards.Count(),
                    
                }).ToList(),
                MyCards = player.Cards.Select(c => new CardDTO() { Id=c.Id, Name=Cards.GetCardById(c.Id).Name }).ToList()
            };
        }

        public bool CreateSession(ICreateSesionDTO sesionDTO, int idHost)
        {
            GameSession newSession = new GameSession(signalrService)
            {
                Name = sesionDTO.Name,
                Private = sesionDTO.Private,
                NewRules = sesionDTO.NewRules,
                IdHost = idHost,
            };

            foreach (var card in Cards.GetAll())
            {
                newSession.Cards.Add(card);
            }


            Player hostPlayer = new Player
            {
                Id = idHost
            };
            newSession.Code = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            newSession.Players.AddLast(hostPlayer);

            sessionsRepository.Insert(newSession);
            return true;

        }

        public void PlayCard(int idPlayer, int cardId)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            session.PlayCard(idPlayer, cardId);
        }

        public void ChangeColor(int idPlayer,ChangeColorDTO dto)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            session.ChangeColor(idPlayer, dto);
            
        }

        public void JoinRandomSession(int id)
        {
            var session = sessionsRepository.GetPublic();
            if (session == null)
            {
                throw new Exception("No public sessions available");
            }
            JoinSessionByCode(session.Code, id);
        }

        public void JoinSessionByCode(string code, int id)
        {
            //if(PlayerSession(id) != null)
            //{
            //    throw new Exception("Player already in a session");
            //}

            var session = sessionsRepository.GetByCode(code);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            Player newPlayer = new Player
            {
                Id = id
            };



            session.Players.AddLast(newPlayer);
            var playerDTO = new PlayerDTO
            {
                Id = newPlayer.Id,
                UserName = usersRepository.Get(newPlayer.Id).Name
            };

            foreach (var player in session.Players)
            {
                if (player.Id != id)
                {
                    
                    signalrService.PlayerJoined(player.Id.ToString(), playerDTO);
                }
            }



        }

        public async Task StartGame(int id)
        {

            var session = sessionsRepository.GetByPlayerId(id);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            if(session.IdHost != id)
            {
                throw new Exception("Only the host can start the game");
            }
            if (session.Started == false)
            {
                session.StartGame();
                foreach (var player in session.Players)
                {
                    await signalrService.GameStarted(player.Id.ToString());
                }
            }

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
