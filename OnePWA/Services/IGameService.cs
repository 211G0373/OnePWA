using OnePWA.Models;

namespace OnePWA.Services
{
    public interface IGameService
    {
        IEnumerable<IGameSesion> Sesions { get; set; }
        ICardsService CardsService { get; }

        void DoMovement(int idPlayer, int card);

        void StartGame();

        void StopGame();

        void ResetGame();





    }
}
