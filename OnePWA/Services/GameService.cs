using OnePWA.Models;

namespace OnePWA.Services
{
    public class GameService : IGameService
    {
        public List<IGameSesion> Sesions { get ; set; }= new List<IGameSesion>();

        public ICardsService CardsService => throw new NotImplementedException();

        public void DoMovement(int idPlayer, int card)
        {
            throw new NotImplementedException();
        }

        public void NextTurn(IGameSesion game)
        {
            throw new NotImplementedException();
        }

        public void OutTumn(IGameSesion game)
        {
            throw new NotImplementedException();
        }

        public void ResetGame()
        {
            throw new NotImplementedException();
        }

        public void ReverseTurn(IGameSesion game)
        {
            throw new NotImplementedException();
        }

        public void SkipTurn(IGameSesion game)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }

        public void StopGame()
        {
            throw new NotImplementedException();
        }
    }
}
