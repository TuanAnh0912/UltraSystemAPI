using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Transactions;
using UltraSystem.Core.Model;

namespace UltraSystem.Core
{
    public class SqlServerContext<T> : IDbContext<T> where T : class
    {
        private DbSettings _dbSettings;
        public SqlServerContext(IOptions<DbSettings> dbsetting)
        {
            _dbSettings = dbsetting.Value;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _dbSettings.ConnectionString;
            return new SqlConnection(connectionString);
        }
        public IDbTransaction GetDbTransaction()
        {
            var db = CreateConnection();
            db.Open();
            return db.BeginTransaction();
        }
        public Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> QueryUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                return await transaction.Connection.QueryAsync<T>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
            using (var db = CreateConnection())
            {
                return await db.QueryAsync<T>(storeName, param: dicParams, commandType: commandType, transaction: transaction);
            }
        }
        public async Task<int> ExcuseUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                var rs = await transaction.Connection.ExecuteAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
            using (var db = CreateConnection())
            {
                var rs = await db.ExecuteAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
        }
        public async Task<object> ExecuteScalarUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction = null,CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                throw (new Exception("Không tồn tại store"));
            }
            dicParams ??= new Dictionary<string, object>();
            if (transaction != null)
            {
                var rs = await transaction.Connection.ExecuteScalarAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
            using (var db = CreateConnection())
            {
                var rs = await db.ExecuteScalarAsync(storeName, param: dicParams, commandType: commandType, transaction: transaction);
                return rs;
            }
        }

        Task<T> IDbContext<T>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<T>> IDbContext<T>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<bool> IDbContext<T>.Add(T entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDbContext<T>.Delete(T entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDbContext<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }

        IDbTransaction IDbContext<T>.GetDbTransaction()
        {
            throw new NotImplementedException();
        }

        IDbConnection IDbContext<T>.CreateConnection()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<X>> IDbContext<T>.QueryUsingStore<X>(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<T>> IDbContext<T>.QueryUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        Task<int> IDbContext<T>.ExcuseUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        Task<object> IDbContext<T>.ExecuteScalarUsingStore(Dictionary<string, object> dicParams, string storeName, IDbTransaction? transaction, CommandType commandType)
        {
            throw new NotImplementedException();
        }
    }
}
