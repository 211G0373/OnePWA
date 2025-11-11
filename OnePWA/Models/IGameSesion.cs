namespace OnePWA.Models
{
    public interface IGameSesion
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        int IdHost { get; set; }
        List<IPlayer> Players { get; set; }
        bool Started { get; set; }
        bool Private { get; set; }
        bool NewRules { get; set; }

        List<int> UsedCards { get; set; }
        List<int> NotUsed { get; set; }
    }
}
