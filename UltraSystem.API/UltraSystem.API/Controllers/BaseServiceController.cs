﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using UltraSystem.Application;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using static UltraSystem.Application.Common.UltraSystemPermisstion;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BaseServiceController<T> : ControllerBase where T : class
    {
        public readonly IBaseService<T> _baseService;
        private Type? _currentType;
        protected Type currentType
        {
            get
            {
                if (_currentType == null)
                {
                    throw new NotImplementedException("chưa gán modal type");
                };
                return _currentType;    
            }
            set
            {
                _currentType = value;
            }
        }
        public BaseServiceController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }
        [HttpPost("add")]
        [Authorize]
        public async Task<ServiceResponse> Add([FromBody] T data)
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "" ;
            var listRole = new List<string>() {$"{tableName}.Add" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Add(data);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<ServiceResponse> Update([FromBody] T data)
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.Update" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Update(data);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<ServiceResponse> Delete([FromRoute] object id)
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.Delete" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Delete(id);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("get/{id}")]
        public async Task<ServiceResponse> GetByID([FromRoute] object id)
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.GetById(id);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("get-all")]
        public async Task<ServiceResponse> GetAll()
        {
            string tableName = currentType.GetCustomAttribute<TableUltraSystemAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.GetAll();
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
    }
}
