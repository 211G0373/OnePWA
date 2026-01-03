namespace OnePWA.Models.DTOs
{
    public interface IWaittingSessionDTO
    {
        string Name { get; set; }
        string Code { get; set; }
        int IdHost { get; set; }
        int PlayerCount { get; set; }

        bool Started { get; set; }

        IEnumerable<IPlayerDTO> Players { get; set; }

    }
}
