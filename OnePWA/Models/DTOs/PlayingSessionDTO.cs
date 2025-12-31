
namespace OnePWA.Models.DTOs
{
    public class PlayingSessionDTO : IPlayingSessionDTO
    {
        public string Name { get ; set ; }
        public int PlayerCount { get ; set ; }
        public int IdTurn { get ; set ; }
        public string LastColor { get ; set ; }

        public int Time { get; set; }

        public bool Reverse { get ; set ; }
        public IEnumerable<IPlayerDTO> Players { get ; set ; } = new List<IPlayerDTO>();
        public CardDTO LastCard { get; set; }
        public List<CardDTO> MyCards { get ; set ; } = new List<CardDTO>();
    }
}
