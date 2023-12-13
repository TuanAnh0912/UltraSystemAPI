using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UltraSystem.Application;
using UltraSystem.Application.Interface;
using UltraSystem.Application.Service;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HardwareController : BaseServiceController<Hardware>
    {
        readonly IHardwareService _hardwareService;
        public HardwareController(IHardwareService hardwareService) : base(hardwareService)
        {
            this.currentType = typeof(Products);
            _hardwareService = hardwareService;
        }
        [Authorize]
        [HttpPost("new-hardware")]
        public async Task<ServiceResponse> AddNewHardWare([FromBody] NewHardwareRequest data)
        {
            //string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { "hardware.Add" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _hardwareService.InsertNewHardware(data.KeyValue, data.HardwareIdentify);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
    }
}