namespace OnePWA.Models.DTOs
{
    public interface IPlayingSessionDTO
    {
        string Name { get; set; }
        int PlayerCount { get; set; }
        IEnumerable<IPlayerDTO> Players { get; set; }
        List<int> MyCards { get; set; }

    }
}
