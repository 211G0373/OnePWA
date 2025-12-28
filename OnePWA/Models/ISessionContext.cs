namespace OnePWA.Models
{
    public interface ISessionContext
    {
        List<IGameSesion> Sesions { get; set; }
        void Remove(IGameSesion session);
    }
}
