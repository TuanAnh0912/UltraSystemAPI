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
    public class LicenseService : BaseService<License>,ILicenseService
    {
        private ILicenseRepository _licenseRepository;
        public LicenseService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _licenseRepository = serviceProvider.GetRequiredService<ILicenseRepository>();
        }
        public async override Task<ServiceResponse> Add(License entity)
        {
            var resInsert = await _licenseRepository.Add(entity);
            if (Convert.ToInt32(resInsert) > 0)
            {
                return new ServiceResponse(true, "Thêm thành công license");
            }
            return new ServiceResponse(false, "Thêm license thất bại");
        }

        public async Task<ServiceResponse> GetLicensesByUserID()
        {
            if (_UserID == Guid.Empty)
            {
                return new ServiceResponse(false, "UserID không tồn tại");
            }
            var lstLicense = await _licenseRepository.GetLicensesByUserID(_UserID);
            return new ServiceResponse(true,"Lấy dữ liệu thành công",lstLicense);
        }
        public async Task<ServiceResponse> GetLicensesByProductID(int productID)
        {
            var lstLicense = await _licenseRepository.GetLicenseByProductID(productID);
            return new ServiceResponse(true, "Lấy dữ liệu thành công", lstLicense);
        }

    }
}
