using OnePWA.Models;

namespace OnePWA.Services
{
    public interface IGameService
    {
        List<IGameSesion> Sesions { get; set; }
        ICardsService CardsService { get; }

        void DoMovement(int idPlayer, int card);

        void NextTurn(IGameSesion game);

        void ReverseTurn(IGameSesion game);

        void SkipTurn(IGameSesion game);

        void StartGame();

        void StopGame();

        void ResetGame();

        void OutTumn(IGameSesion game);



    }
}
