using Microsoft.AspNetCore.Mvc;
using UltraSystem.Application.Interface;
using UltraSystem.Application.Service;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Application;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenseController : BaseServiceController<License>
    {
        readonly ILicenseService _licenseService;
        public LicenseController(ILicenseService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            this.currentType = typeof(License);
            _licenseService = serviceProvider.GetRequiredService<ILicenseService>();
        }
        [HttpGet("licenses-by-user")]
        [Authorize]
        public async Task<ServiceResponse> GetLisenseByUserD()
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _licenseService.GetLicensesByUserID();
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("licenses-by-product")]
        [Authorize]
        public async Task<ServiceResponse> GetLisenseByProduct([FromQuery]int productID)
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _licenseService.GetLicensesByProductID(productID);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
    }
}