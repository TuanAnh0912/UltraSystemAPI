using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UltraSystem.Core.Helpers;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using static Dapper.SqlMapper;

namespace UltraSystem.Core.Repositories
{
    public class PurchasedProductRepository : GenericRepositories<PurchasedProduct>, IPurchasedProductRepository
    {
        private IKeyRepository _keyRepository;
        private IUserRepository _userRepository;
        public PurchasedProductRepository(IDbContext<PurchasedProduct> dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            _keyRepository = serviceProvider.GetRequiredService<IKeyRepository>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        }
        public async Task<Decimal> CheckAcountBalance(int licenseID,Guid userID)
        {
            var param = new Dictionary<string, object>()
            {
               {"v_LicenseID",licenseID },
               {"v_UserID",userID },
            };
            var checkrs = await _dbContext.ExecuteScalarUsingStore(param, "Proc_GetAcountBalance");
            return Convert.ToDecimal(checkrs);
        }
        public async Task<ServiceResponse> InsertPurchasedProduct(PurchasedProduct entity)
        {
            var acountBalanceAfterBuy = await CheckAcountBalance(entity.LicenseID, entity.UserID);
            if (acountBalanceAfterBuy < 0)
            {
                return new ServiceResponse(false, "Số dư trong tài khoản không đủ");
            }
            var param = new Dictionary<string, object>()
            {
               {"v_LicenseID",entity.LicenseID },
               {"v_PurchaseDate",entity.PurchaseDate },
               {"v_UserID",entity.UserID },
               {"v_KeyID" ,entity.KeyID},
               {"v_ProductID",entity.ProductID }
            };
            using (var db = _dbContext.CreateConnection())
            {
                var transaction = _dbContext.GetDbTransaction();
                // update số dư tài khoản sau khi mua license
                var updateUser = await _userRepository.UpdateAcountBalance(acountBalanceAfterBuy, entity.UserID);
                if (!updateUser)
                {
                    transaction.Rollback();
                    return new ServiceResponse(false, "có lỗi xảy ra");
                }
                var keyvalue = await _keyRepository.GetNewKey(transaction);
                //Thêm mới sản phẩm mua và trả và keyvalue
                var resInsert = await _dbContext.ExecuteScalarUsingStore(param, "Proc_InsertPurchasedProduct", transaction);
                if (Convert.ToInt32(resInsert) > 0)
                {
                    var keyModel = new Key()
                    {
                        PurchasedProductID = Convert.ToInt32(resInsert),
                        KeyValue = keyvalue
                    };
                    var resInsertKey = await _keyRepository.Add(keyModel);
                    if (!string.IsNullOrEmpty(resInsertKey.ToString()))
                    {
                        entity.KeyID = Convert.ToInt32(resInsertKey.ToString());
                        var cloumnUpdate = new List<string>() { "KeyID" };
                        var condition = $"PurchasedProductID = {resInsert}";
                        //update lại keyid vào productpurchased
                        var rsUpdatePurchased = await UpdateCustomColumn(entity, cloumnUpdate, condition, transaction);
                        if (rsUpdatePurchased)
                        {
                            transaction.Commit();
                            var rsData = new
                            {
                                PurchasedProductID = resInsert,
                                KeyValue = keyModel.KeyValue
                            };
                            return new ServiceResponse(true, "Mua thành công",rsData);
                        }
                    }
                }
                transaction.Rollback();
            }
            return new ServiceResponse(false,"Có lỗi xảy ra");
        }
        public override string GetTableName()
        {
            return "purchasedproduct";
        }
        public async Task<string> RefreshKeyForLicens(PurchasedProduct purchasedProduct, bool isDeleteHardware = true)
        {
            var keyRefrest = await _keyRepository.UpdateKeyDelHardWareByKeyID(purchasedProduct, isDeleteHardware);
            if (!string.IsNullOrEmpty(keyRefrest.ToString()))
            {
                return keyRefrest.ToString() ?? "";
            }
            return "";
        }
    }
}
