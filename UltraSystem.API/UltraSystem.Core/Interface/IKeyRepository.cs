using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;

namespace UltraSystem.Core.Interface
{
    public interface IKeyRepository : IGenericRepository<Key>
    {
        Task<object> UpdateKeyDelHardWareByKeyID(PurchasedProduct purchasedProduct, bool isDeleteHardware = true);
        Task<Dictionary<string, object>> CheckInsertHardware(string keyValue);
        Task<string> GetNewKey(IDbTransaction dbTransaction = null);
    }
}
