using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;

namespace UltraSystem.Core.Repositories
{
    public class LicenseRepository : GenericRepositories<License>, ILicenseRepository
    {
        public LicenseRepository(IDbContext<License> dbContext) : base(dbContext)
        {
        }
        public override string GetTableName()
        {
            return "license";
        }
        public async override Task<object> Add(License entity)
        {
            var param = new Dictionary<string, object>()
            {
               {"v_ProductID",entity.ProductID },
               {"v_IsActive",entity.IsActive },
               {"v_ExpiryDate",entity.ExpiryDate },
               {"v_Price" ,entity.Price},
               {"v_MaxLicenseCount",entity.MaxLicenseCount }
            };
            var resInsert = await _dbContext.ExcuseUsingStore(param, "Proc_InsertLicense");
            return resInsert;
        }
        public async Task<List<License>> GetLicenseByProductID(int productID)
        {
            var param = new Dictionary<string, object>()
            {
                {"@productID",productID }
            };
            var sql = "select * from license where ProductID =@productID;";
            var licensesByProductID = await _dbContext.QueryUsingStore(param, sql,commandType: CommandType.Text);
            return licensesByProductID.ToList();
        }
        public async Task<List<License>> GetLicensesByUserID(Guid userID)
        {
            var param = new Dictionary<string, object>()
            {
                {"v_UserID",userID }
            };
            var licensesByUserID = await _dbContext.QueryUsingStore(param, "Proc_GetLicensesByUserID");
            return licensesByUserID.ToList();
        }
       
    }
}
