using OnePWA.Models.Entities;
using System.Timers;
using Timer = System.Timers.Timer;

namespace OnePWA.Models
{
    public interface IGameSesion
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        int IdHost { get; set; }
        LinkedList<IPlayer> Players { get; set; }
        bool Started { get; set; }
        bool Private { get; set; }
        bool NewRules { get; set; }
        Timer Timer { get; set; }
        public int IdTurn { get; set; }
        List<int> UsedCards { get; set; }
        List<int> NotUsed { get; set; }



        List<Cards> Cards { get; set; }
        void DoMovement(int idPlayer, int card);

        void NextTurn();

        void ReverseTurn();

        void SkipTurn();

        void StartGame();

        void StopGame();

        void ResetGame();









    }
}
