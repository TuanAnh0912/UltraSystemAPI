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
    public class HardwareService : BaseService<Hardware>, IHardwareService
    {
        readonly IHardwareRepository _hardwareRepository;
        readonly IKeyRepository _keyRepository;
        public HardwareService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _hardwareRepository = serviceProvider.GetRequiredService<IHardwareRepository>();
            _keyRepository = serviceProvider.GetRequiredService<IKeyRepository>();
        }
        public async Task<ServiceResponse> DeleteHardWareByKeyID(int keyID)
        {
            var resDelete = await _hardwareRepository.DeleteHardWareByKeyID(keyID);
            if (resDelete)
            {
                return new ServiceResponse(true, "Xóa thành công");
            }
            return new ServiceResponse(false, "Xóa thất bại");
        }
        public async Task<ServiceResponse> InsertNewHardware(string keyValue,string hardwareIdentify)
        {
            var checkExistsHardware = await _hardwareRepository.CheckExistsHardware(keyValue, hardwareIdentify);
            if (checkExistsHardware)
            {
                return new ServiceResponse(true, "Máy đã được tạo trước đó");
            }
            var resCheck = await _keyRepository.CheckInsertHardware(keyValue);
            if (resCheck != null)
            {
                var hardwareModel = new Hardware() 
                {
                    KeyID = int.Parse(resCheck["keyID"]?.ToString() ?? "0"),
                    HardwareIdentify = hardwareIdentify,
                    HardwareName = "",
                };
                var resInsert = await _hardwareRepository.Add(hardwareModel);
                if (Convert.ToBoolean(resInsert))
                {
                    return new ServiceResponse(true, "Thêm thành công");
                }
                else
                {
                    return new ServiceResponse(false, "Thêm thất bại");
                }
            }
            else
            {
                return new ServiceResponse(false, "Số lượng máy vượt quá số lượng license có");

            }
        }

    }
}
