namespace OnePWA.Models
{
    public interface IGameSesion
    {
        string Name { get; }
        List<IPlayer> Players { get; }
        bool Started { get; set; }
        bool IsPublic { get; set; }
        List<int> UsedCards { get; set; }
        List<int> NotUsed { get; set; }





    }
}
