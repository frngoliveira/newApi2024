using System.Linq.Expressions;

namespace FRN.Domain._2._1_Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity entity);
        TEntity GetById(int id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        void Update(TEntity entity);
        void Remove(int id);
        int SaveChanges();

    }
}
