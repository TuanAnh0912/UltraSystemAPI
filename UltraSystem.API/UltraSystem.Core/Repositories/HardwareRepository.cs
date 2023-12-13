using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;

namespace UltraSystem.Core.Repositories
{
    public class HardwareRepository : GenericRepositories<Hardware>, IHardwareRepository
    {
        public HardwareRepository(IDbContext<Hardware> dbContext) : base(dbContext)
        {
        }
        public async Task<bool> DeleteHardWareByKeyID(int keyid, IDbTransaction transaction = null)
        {
            var query = $"DELETE FROM hardware WHERE KeyID = @keyID";
            var param = new Dictionary<string, object>()
            {
                {"@keyID",keyid }
            };
            var resDeleteHardWare = await _dbContext.ExcuseUsingStore(param,query, transaction,CommandType.Text);
            return resDeleteHardWare > 0;
        }
        public async Task<bool> CheckExistsHardware(string keyValue, string hardwareIdentify)
        {
            var param = new Dictionary<string, object>()
            {
                {"@keyValue" ,keyValue},
                {"@hardwareIdentify" ,hardwareIdentify},
            };
            var sql = "select * from  hardware h join `key`  k ON h.KeyID = k.KeyID WHERE k.KeyValue = @keyValue and h.HardwareIdentify = @hardwareIdentify;";
            var rsGetHardware = await _dbContext.QueryUsingStore(param, sql, commandType: CommandType.Text);
            if (rsGetHardware.Any())
            {
                return true;
            }
            return false;
        }
        public override string GetTableName()
        {
            return "hardware";
        }

    }
}
