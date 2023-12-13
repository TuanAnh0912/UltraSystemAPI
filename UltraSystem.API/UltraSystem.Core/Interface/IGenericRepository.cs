using System.Data;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(object ID);
        Task<IEnumerable<T>> GetAll();
        Task<object> Add(T entity);
        Task<bool> Delete(object ID);
        Task<bool> Update(T entity);
        Task<bool> UpdateCustomColumn(T entity, List<string> columnsToUpdate, string condition, IDbTransaction transaction = null);
        string GetTableName();
        Task<object> MultiInsert(List<BaseModel> data, IDbTransaction dbTransaction, bool selectKey);
    }
}
