using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Repositories;

namespace OnePWA.Services
{
    public interface ISessionsService
    {
        IRepository<Users> usersRepository { get; }
        ISessionsRepository sessionsRepository { get; }

        SignalrService signalrService { get; }

        IWaittingSessionDTO PlayerSession(int id);
        IPlayingSessionDTO PlayingSession(int id);
        bool CreateSession(ICreateSesionDTO sesionDTO, int idHost);
        void JoinSessionByCode(string code,int id);

        Task StartGame(int id);
        void PlayCard(int idPlayer, int cardId);
        void BlackCard(int idPlayer, ChangeColorDTO dto);
        void JoinRandomSession(int id);
        void PlayAgain(int id);
        void RemovePlayerFromSession(int idPlayer, int idPlayerForRemove);
        void LeaveSession(int idPlayer);
    }
}
