namespace OnePWA.Models.DTOs
{
    public interface IPlayingSessionDTO
    {
        string Name { get; set; }
        int PlayerCount { get; set; }
        
        int IdTurn { get; set; }
        IEnumerable<IPlayerDTO> Players { get; set; }
        List<CardDTO> MyCards { get; set; }

    }
}
