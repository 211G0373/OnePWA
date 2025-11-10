namespace OnePWA.Models
{
    public interface IGameSesion
    {
        string Name { get; set; }

        int IdHost { get; set; }
        List<IPlayer> Players { get; set; }
        bool Started { get; set; }
        bool IsPublic { get; set; }
        List<int> UsedCards { get; set; }
        List<int> NotUsed { get; set; }
    }
}
