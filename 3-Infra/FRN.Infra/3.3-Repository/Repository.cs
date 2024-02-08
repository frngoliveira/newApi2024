using FRN.Domain._2._1_Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FRN.Infra._3._3_Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(DbContext context) 
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        } 

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual TEntity GetById(int id) 
        {
            return DbSet.Find();
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) 
        {
            return DbSet.AsNoTracking().Where(predicate);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(TEntity obj) 
        {
            DbSet.Update(obj);
        }

        public virtual void Remove(int id) 
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
