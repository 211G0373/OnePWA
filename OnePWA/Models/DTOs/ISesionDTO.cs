namespace OnePWA.Models.DTOs
{
    public interface ISesionDTO
    {
        string Name { get; }
        string Code { get; }

        int HostId { get; }

        int PlayerCount { get; }

        IEnumerable<IPlayerDTO> Players { get; }

    }
}
