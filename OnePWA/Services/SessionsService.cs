
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
                Started = session.Started,
                PlayerCount = session.Players.Count(),
                Time = (int)(session.AutoStartTimer.Interval - (DateTime.Now - session.AutostartTime).TotalMilliseconds),
                Players = session.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    UserName = usersRepository.Get(p.Id).Name,
                    CardsCount = p.Cards.Count(),
                    FotoPerfil = usersRepository.Get(p.Id).ProfilePictures,
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
                    FotoPerfil = usersRepository.Get(p.Id).ProfilePictures
                }).ToList(),
                MyCards = player.Cards.Select(c => new CardDTO() { Id = c.Id, Color = Cards.GetCardById(c.Id).Color, Name = Cards.GetCardById(c.Id).Name }).ToList(),
                LastCard = new CardDTO() { Id = session.TopCard.Id, Color = session.LastColor, Name = Cards.GetCardById(session.TopCard.Id).Name },
                LastColor = session.LastColor,
                Reverse = session.IsReversed,
                Time = (int)(session.Timer.Interval - (DateTime.Now - session.PlayerstartTime).TotalMilliseconds),

            };

        }

        public async Task EliminarUsuario(int idPlayer)
        {

        }




        public async Task<bool> CreateSession(ICreateSesionDTO sesionDTO, int idHost)
        {
            var existingSession = sessionsRepository.GetByPlayerId(idHost);
            if (existingSession != null)
            {
                await existingSession.playerOut(idHost);
            }

            GameSession newSession = new GameSession(signalrService, sessionsRepository.Context)
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

        public void Replay(int idPlayer)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            session.RePlayGame(idPlayer);


            var playerDTO = new PlayerDTO
            {
                Id = idPlayer,
                UserName = usersRepository.Get(idPlayer).Name,
                FotoPerfil = usersRepository.Get(idPlayer).ProfilePictures
            };

            foreach (var player in session.Players)
            {
                if (player.Id != idPlayer)
                {

                    _= signalrService.PlayerJoined(player.Id.ToString(), playerDTO);
                }
            }


        }

        public async Task PlayCard(int idPlayer, int cardId)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            if (session.IdTurn != idPlayer)
            {
                throw new Exception("It's not your turn");
            }
            try
            {
                await session.PlayCard(idPlayer, cardId);
            }
            catch
            {
                throw new Exception("It's not your turn");
            }
        }

        public async Task BlackCard(int idPlayer, ChangeColorDTO dto)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            try
            {
                await session.BlackCard(idPlayer, dto);
            }
            catch
            {
                throw new Exception("It's not your turn");
            }

        }

        public async Task JoinRandomSession(int id)
        {
            var session = sessionsRepository.GetPublic();
            if (session == null)
            {
                throw new Exception("No public sessions available");
            }
            await JoinSessionByCode(session.Code, id);
        }

        public async Task JoinSessionByCode(string code, int id)
        {
            var existingSession = sessionsRepository.GetByPlayerId(id);
            if (existingSession!=null)
            {
                await existingSession.playerOut(id);
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



            session.Players.AddLast(newPlayer);
            var playerDTO = new PlayerDTO
            {
                Id = newPlayer.Id,
                UserName = usersRepository.Get(newPlayer.Id).Name,
                FotoPerfil = usersRepository.Get(newPlayer.Id).ProfilePictures
            };

            foreach (var player in session.Players)
            {
                if (player.Id != id)
                {

                    await signalrService.PlayerJoined(player.Id.ToString(), playerDTO);
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
            if (session.IdHost != id)
            {
                throw new Exception("Only the host can start the game");
            }
            if(session.Players.Count < 2)
            {
                throw new Exception("At least 2 players are required to start the game");
            }
            if (session.Started == false)
            {
                session.StartGame();
                
            }

        }


        public void LeaveSession(int idPlayer)
        {
            throw new NotImplementedException();
        }
        public void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove)
        {
            var session = sessionsRepository.GetByPlayerId(idPlayer);
            if (session == null)
            {
                throw new Exception("Session not found");
            }
            if (session.IdHost != idPlayer)
            {
                throw new Exception("Only the host can start the game");
            }
            if (!session.Players.Any(X => X.Id == idPlayerForRemove))
            {
                throw new Exception("EL jugador no esta en la partida");
            }
            if (session.Started)
            {
                throw new Exception("La partida ya inicio");
            }
            session.playerOut(idPlayerForRemove);




        }




        public void PlayAgain(int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
