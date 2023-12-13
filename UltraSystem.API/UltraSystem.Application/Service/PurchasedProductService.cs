using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Service
{
    public class PurchasedProductService : BaseService<PurchasedProduct>, IPurchasedProductService
    {
        private IPurchasedProductRepository _purchasedProductRepository;
        private ILicenseRepository _licenseRepository;
        public PurchasedProductService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _purchasedProductRepository = serviceProvider.GetRequiredService<IPurchasedProductRepository>();
            _licenseRepository = serviceProvider.GetRequiredService<ILicenseRepository>();
        }
        public async override Task<ServiceResponse> Add(PurchasedProduct entity)
        {
            entity.UserID = _UserID;
            return await _purchasedProductRepository.InsertPurchasedProduct(entity);
        }
        public async Task<ServiceResponse> GetNewKeyForLicense(int purchaseID, bool isDeleteHardware = true)
        {
            var purchase = await _purchasedProductRepository.GetById(purchaseID);
            if (purchase == null)
            {
                return new ServiceResponse(false, "Sản phẩm chưa được mua");
            }
            var newKey = await _purchasedProductRepository.RefreshKeyForLicens(purchase, isDeleteHardware);
            return new ServiceResponse(true, "", newKey);
        }
    }
}
