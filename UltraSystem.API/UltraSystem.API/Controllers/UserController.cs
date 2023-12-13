using Microsoft.AspNetCore.Mvc;
using UltraSystem.Application.Interface;
using UltraSystem.Application.Service;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Application;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using UltraSystem.Core.Model.RequestModel;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : BaseServiceController<User>
    {
        readonly IHardwareService _hardwareService;
        readonly IPurchasedProductService _purchasedProductService;
        readonly IUserService _userService;
        public UserController(IUserService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            _hardwareService = serviceProvider.GetRequiredService<IHardwareService>();
            _purchasedProductService = serviceProvider.GetRequiredService<IPurchasedProductService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            this.currentType = typeof(License);
        }
        [HttpDelete("keys/{keyID}/hardware")]
        public async Task<ServiceResponse> DeleteByKeyID([FromRoute] int keyID)
        {
            var listRole = new List<string>() { $"hardware.Delete" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _hardwareService.DeleteHardWareByKeyID(keyID);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("purchases/{purchaseID}/reset-key")]
        public async Task<ServiceResponse> GetNewKey([FromRoute] int purchaseID)
        {
            var listRole = new List<string>() { $"key.Update" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _purchasedProductService.GetNewKeyForLicense(purchaseID,false);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("user-license")]
        public async Task<ServiceResponse> GetUserLicense()
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _userService.GetUserLicenses();
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<ServiceResponse> CreateUser([FromBody]User user)
        {
            return await _baseService.Add(user);
        }
        [HttpGet("role-permission")]
        public async Task<ServiceResponse> GetAllRole()
        {
            var listRole = new List<string>() { "role.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {     
                return await _userService.GetAllRole();
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpPost("new-role")]
        public async Task<ServiceResponse> AddRolePermisstion([FromBody] RolePermisstionRequestModel model)
        {
            var listRole = new List<string>() { "role.Add" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _userService.InsertRolePermisstion(model);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("link-reset")]
        public Task<ServiceResponse> GetLinkResetPassword()
        {
            return _userService.SendMail();
        }
        [HttpPost("reset-password")]
        public Task<ServiceResponse> GetLinkResetPassword(RequestResetPasswordModel data)
        {
            return _userService.ResetPassword(data.NewPassword);
        }
    }
  
}