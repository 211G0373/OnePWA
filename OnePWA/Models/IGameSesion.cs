namespace OnePWA.Models
{
    public interface IGameSesion
    {
        string Name { get; }
        List<IPlayer> Players { get; }

        void AddPlayer(IPlayer player);
        void RemovePlayer(IPlayer player);

        void StartGame();




    }
}
