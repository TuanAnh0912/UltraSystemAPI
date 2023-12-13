using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Interface
{
    public interface IPurchasedProductRepository:IGenericRepository<PurchasedProduct>
    {
        Task<string> RefreshKeyForLicens(PurchasedProduct purchasedProduct, bool isDeleteHardware = true);
        Task<ServiceResponse> InsertPurchasedProduct(PurchasedProduct entity);
    }
}
