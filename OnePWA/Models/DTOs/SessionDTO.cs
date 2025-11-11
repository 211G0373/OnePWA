

namespace OnePWA.Models.DTOs
{
    public class SessionDTO : ISessionDTO
    {
        public string Name { get ; set ; }
        public string Code { get ; set ; }
        public int IdHost { get ; set ; }
        public int PlayerCount { get ; set ; }
        public IEnumerable<IPlayerDTO> Players { get ; set ; }= new List<IPlayerDTO>();
    }
}
