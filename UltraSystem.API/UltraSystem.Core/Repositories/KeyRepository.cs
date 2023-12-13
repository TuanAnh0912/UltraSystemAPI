using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using System.Data.SqlClient;
using UltraSystem.Core.Helpers;
using System.Data;
using System.Transactions;

namespace UltraSystem.Core.Repositories
{
    public class KeyRepository : GenericRepositories<Key>, IKeyRepository
    {
        readonly IHardwareRepository _hardwareRepository;
        public KeyRepository(IDbContext<Key> dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            _hardwareRepository = serviceProvider.GetRequiredService<IHardwareRepository>();
        }
        public async override Task<object> Add(Key entity)
        {
            var param = new Dictionary<string, object>() {
                {"v_PurchasedProductID",entity.PurchasedProductID },
                {"v_KeyValue",entity.KeyValue }
            };
            var resInsert = await _dbContext.ExecuteScalarUsingStore(param, "Proc_InsertKey");
            return resInsert;
        }
        public async Task<string> GetNewKey(IDbTransaction dbTransaction = null)
        {
            var getAllKeyValue = new List<string>();
            var keyvalue = StringHelper.GenerateRandomString();
            var sql = "SELECT k.KeyValue FROM `key` k";
            if (dbTransaction == null)
            {
                using (var db = _dbContext.CreateConnection())
                {
                    db.Open();
                    getAllKeyValue = await db.QueryAsync<string>(sql) as List<string>;
                }
            }
            else
            {
                getAllKeyValue = await dbTransaction.Connection.QueryAsync<string>(sql) as List<string>;
            }
            while (getAllKeyValue.Contains(keyvalue))
            {
                keyvalue = StringHelper.GenerateRandomString();
            }
            return keyvalue;
        }
        public async Task<object> UpdateKeyDelHardWareByKeyID(PurchasedProduct purchasedProduct, bool isDeleteHardware = true)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                var transaction = _dbContext.GetDbTransaction();
                if (isDeleteHardware)
                {
                    await _hardwareRepository.DeleteHardWareByKeyID(purchasedProduct.KeyID, transaction);
                }
                var keyvalue = await GetNewKey(transaction);
                var modelUpdateKey = new Key()
                {
                    KeyID = purchasedProduct.KeyID,
                    PurchasedProductID = purchasedProduct.PurchasedProductID,
                    KeyValue = keyvalue
                };
                var resUpdateKey = await Update(modelUpdateKey);
                if (resUpdateKey)
                {
                    transaction.Commit();
                    return modelUpdateKey.KeyValue;
                }
                transaction.Rollback();
            }
            return null;
        }
        public override string GetTableName()
        {
            return "Key";
        }
        public async Task<Dictionary<string, object>> CheckInsertHardware(string keyValue)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_KeyValue",keyValue}
            };
            var dicParam = new Dictionary<string, object>();
            using (var db = _dbContext.CreateConnection())
            {
                using (var reader = await db.ExecuteReaderAsync("Proc_GetInforByKeyValue", param: param, commandType: System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        dicParam.Add("hardwareUsing", reader.GetInt32(0));
                        dicParam.Add("maxcountLicense", reader.GetInt32(1));
                        dicParam.Add("keyID", reader.GetInt32(2));
                        break;
                    }
                }
                if (int.Parse(dicParam["hardwareUsing"]?.ToString() ?? "0") >= int.Parse(dicParam["maxcountLicense"]?.ToString() ?? "0")) return null;
            }
            return dicParam;
        }
    }
}
