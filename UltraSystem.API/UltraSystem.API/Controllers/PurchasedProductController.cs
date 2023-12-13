using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UltraSystem.Application;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchasedProductController : BaseServiceController<PurchasedProduct>
    {
        readonly IPurchasedProductService _purchasedProductService;

        public PurchasedProductController(IPurchasedProductService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            _purchasedProductService = serviceProvider.GetRequiredService<IPurchasedProductService>();
            currentType = typeof(PurchasedProduct);
        }
        [HttpGet("/purchases/{purchaseID}/keys")]
        public async Task<ServiceResponse> GetNewKeyWithDeleteHardware([FromRoute] int purchaseID)
        {
            var listRole = new List<string>() { "key.Update", "hardware.Delete" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _purchasedProductService.GetNewKeyForLicense(purchaseID);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("key/{licenseID}")]
        public async Task<ServiceResponse> BuyLicenseGetKey([FromRoute] int licenseID)
        {
            var listRole = new List<string>() { "purchasedproduct.Add" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                var purchasedProduct = new PurchasedProduct()
                {
                    LicenseID = licenseID
                };
                return await _purchasedProductService.Add(purchasedProduct);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
    }
}