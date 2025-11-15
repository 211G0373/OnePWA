
namespace OnePWA.Models.DTOs
{
    public class PlayingSessionDTO : IPlayingSessionDTO
    {
        public string Name { get ; set ; }
        public int PlayerCount { get ; set ; }
        public IEnumerable<IPlayerDTO> Players { get ; set ; } = new List<IPlayerDTO>();
        public List<int> MyCards { get ; set ; } = new List<int>();
    }
}
