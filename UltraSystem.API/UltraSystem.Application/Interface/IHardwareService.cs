using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Interface
{
    public interface IHardwareService:IBaseService<Hardware>
    {
        Task<ServiceResponse> DeleteHardWareByKeyID(int keyID);
        Task<ServiceResponse> InsertNewHardware(string keyValue, string HardwareIdentify);
    }
}
