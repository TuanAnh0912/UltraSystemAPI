using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Interface
{
    public interface IPurchasedProductService:IBaseService<PurchasedProduct>
    {
        Task<ServiceResponse> GetNewKeyForLicense(int purchaseID, bool isDeleteHardware = true);
    }
}
