using OnePWA.Models;

namespace OnePWA.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        public SessionsRepository(ISessionContext context)
        {
            Context = context;
        }

        public ISessionContext Context { get; }


        public void Delete(int id)
        {
            var session = Context.Sesions.FirstOrDefault(x => x.Id == id);
            if (session != null)
            {
                Context.Sesions.Remove(session);
            }
        }

        public IEnumerable<IGameSesion> GetAll()
        {
            return Context.Sesions;
        }

        public IGameSesion? GetByCode(string id)
        {
            var session = Context.Sesions.FirstOrDefault(x => x.Code == id);
            return session;
        }


        public IGameSesion GetById(int id)
        {
            var session = Context.Sesions.FirstOrDefault(x => x.Id == id);
            return session;
        }

        public IGameSesion GetByPlayerId(int id)
        {
            var session = Context.Sesions.FirstOrDefault(x => x.Players.Any(p => p.Id == id));
            return session;
        }

        public IEnumerable<IGameSesion> GetPublic()
        {
            var sessions = Context.Sesions.Where(x => !x.Private);
            return sessions;
        }

        public void Insert(IGameSesion entity)
        {
            Context.Sesions.Add(entity);
        }
    }
}
