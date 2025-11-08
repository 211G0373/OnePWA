
using OnePWA.Models.Entities;

namespace OnePWA.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(OnecgdbContext context)
        {
            Context = context;
        }

        public OnecgdbContext Context { get; }

        public void Delete(object id)
        {
            var entity = Context.Find<T>(id);
            if (entity != null)
            {
                Context.Remove(entity);
                Context.SaveChanges();
            }
        }

        public T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
    }
}
