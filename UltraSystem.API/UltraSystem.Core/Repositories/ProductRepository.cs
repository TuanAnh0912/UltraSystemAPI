using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.ResponseModel;

namespace UltraSystem.Core.Repositories
{
    public class ProductRepository : GenericRepositories<Products>, IProductRepository
    {
        public ProductRepository(IDbContext<Products> dbContext) : base(dbContext)
        {
        }
        public async override Task<object> Add(Products entity)
        {
            var param = new Dictionary<string, object>() {
                {"v_ProductName",entity.ProductName },
                {"v_Description",entity.Description }
            };
            var resInsert = await _dbContext.ExcuseUsingStore(param, "Proc_InsertProduct");
            return resInsert;
        }
        public async Task<IEnumerable<ProductFromDbModel>> GetAllProductLicens()
        {
            using (var db = _dbContext.CreateConnection())
            {
                var sql = "select * from product as p join license as l on p.ProductID = l.ProductID";
                //todo: chỗ này trả về kiểu 
                //        "data": [
                //{
                //            "productID": 1,
                //        "productName": "UltraLogin",
                //        "description": "Sản phẩm",
                //        "licenses": [
                //        {
                //                "licenseID": 1,
                //            "productID": 0,
                //            "isActive": true,
                //            "expiryDate": "2023-09-10T16:55:05",
                //            "price": 10000.00,
                //            "maxLicenseCount": 3
                //        }
                //        ] như này dùm anh
                var resGetAll = await db.QueryAsync<ProductFromDbModel>(sql, commandType: CommandType.Text);
                return resGetAll;
            }
        }
    }
}
