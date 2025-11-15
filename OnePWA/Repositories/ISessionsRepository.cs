using OnePWA.Models;

namespace OnePWA.Repositories
{
    public interface ISessionsRepository
    {
        IEnumerable<IGameSesion> GetAll();
        IGameSesion GetPublic();

        IGameSesion GetById(int id);
        IGameSesion GetByPlayerId(int id);
        IGameSesion GetByCode(string id);
        void Insert(IGameSesion entity);
        //void Update(T entity);
        void Delete(int id);
    }
}
