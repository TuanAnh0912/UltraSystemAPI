using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UltraSystem.Application;
using UltraSystem.Application.Interface;
using UltraSystem.Application.Service;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyController : BaseServiceController<Key>
    {
        readonly IHardwareService _hardwareService;
        public KeyController(IKeyService keyService, IHardwareService hardwareService) : base(keyService)
        {
            this.currentType = typeof(Key);
            _hardwareService = hardwareService;
        }
    }
}