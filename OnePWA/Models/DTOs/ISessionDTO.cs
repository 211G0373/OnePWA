namespace OnePWA.Models.DTOs
{
    public interface ISessionDTO
    {
        string Name { get; set; }
        string Code { get; set; }

        int IdHost { get; set; }

        int PlayerCount { get; set; }

        IEnumerable<IPlayerDTO> Players { get; set; }

    }
}
