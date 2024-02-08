using FRN.Domain._2._1_Interface.DapperInterface;
using System.Data;
using Dapper;

namespace FRN.Infra._3._3_Repository.DapperRepository
{
    public abstract class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {

        protected readonly IDbConnection dbConn;
        protected readonly IDbTransaction dbTransaction;

        protected abstract string InsertQuery { get; }
        protected abstract string InsertQueryReturnInserted { get; }
        protected abstract string UpdateByIdQuery { get; }
        protected abstract string DeleteByIdQuery { get; }  
        protected abstract string SelectAllQuery { get; }
        protected abstract string SelectByIdQuery { get; }

        protected RepositoryAsync(IDatabaseFactory databaseOptions)
        {
            dbConn = databaseOptions.GetDbConnection;
            dbConn.Open();
        }

        protected RepositoryAsync(IDbConnection databaseConnection, IDbTransaction transaction = null)
        {
            dbConn = databaseConnection;
            if(dbConn.State != ConnectionState.Open) 
                dbConn.Open();
            dbTransaction = transaction;
        }

        public void Dispose()
        {
            dbConn.Close();
            dbConn.Dispose();   
            GC.SuppressFinalize(this);
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            TEntity entity = await dbConn.QuerySingleAsync<TEntity>(InsertQueryReturnInserted, obj, transaction: dbTransaction);
            return entity;
        }
        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return await dbConn.ExecuteAsync(InsertQuery, entities, transaction: dbTransaction);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbConn.QueryAsync<TEntity>(SelectAllQuery, transaction: dbTransaction);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            var entity = await dbConn.QueryAsync<TEntity>(SelectByIdQuery, new { id = id }, transaction: dbTransaction);
            return entity.FirstOrDefault();
        }

        public virtual async Task<bool> RemoveAsync(object id)
        {
            var entity =  await GetByIdAsync(id);

            if(entity == null)
                return false;

            return await RemoveAsync(entity) > 0 ? true : false;
        }

        public virtual async Task<int> RemoveAsync(TEntity obj)
        {
            return await dbConn.ExecuteAsync(DeleteByIdQuery, null, transaction: dbTransaction);
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            return await dbConn.ExecuteAsync(DeleteByIdQuery, null, transaction: dbTransaction);
        }

        public virtual async Task<int> UpdateAsync(TEntity obj)
        {
            return await dbConn.ExecuteAsync(UpdateByIdQuery, obj, transaction: dbTransaction);
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return await dbConn.ExecuteAsync(UpdateByIdQuery, entities.Select(obj => obj), transaction: dbTransaction);
        }






    }
}
