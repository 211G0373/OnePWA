
namespace OnePWA.Models
{
    public class SessionContext : ISessionContext
    {
        public List<IGameSesion> Sesions { get; set; }= new List<IGameSesion>();

        public void Remove(IGameSesion session)
        {
            Sesions.Remove(session);
        }
    }
}
