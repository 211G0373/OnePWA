
namespace OnePWA.Models
{
    public class GameSession : IGameSesion
    {
        public string Name { get; set; }
        public int IdHost { get; set; }
        public List<IPlayer> Players { get; set; }=new List<IPlayer>();
        public bool Started { get; set; }
        public bool IsPublic { get; set; }
        public List<int> UsedCards { get; set; } = new List<int>();
        public List<int> NotUsed { get; set; } = new List<int>();
    }
}
