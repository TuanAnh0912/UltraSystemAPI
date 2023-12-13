using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Model.ResponseModel;

namespace UltraSystem.Application.Service
{
    public class ProductsService : BaseService<Products>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductsService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _productRepository = serviceProvider.GetRequiredService<IProductRepository>();
        }
        public async override Task<ServiceResponse> Add(Products entity)
        {
            var resInsert = await _productRepository.Add(entity);
            if (Convert.ToInt32(resInsert) > 0)
            {
                return new ServiceResponse(true, "Thêm thành công sản phẩm");
            }
            return new ServiceResponse(false, "Thêm sản phẩm thất bại");
        }
        public async override Task<ServiceResponse> GetAll()
        {
            var datas = await _productRepository.GetAllProductLicens();
            if (datas == null)
            {
                return new ServiceResponse(false, "Lấy dữ liệu thất bại");
            }
            var dicProduct = datas.GroupBy(x => x.ProductID).ToDictionary(g => g.Key, g => g.ToList());
            var rsData = new List<ProductLicensResponseModel>();
            foreach (var item in dicProduct)
            {
                var modelProuctLicens = new ProductLicensResponseModel();
                modelProuctLicens.ProductID = item.Key;
                var lstLicense = new List<License>();
                foreach (var product in item.Value)
                {
                    modelProuctLicens.ProductName = product.ProductName;
                    modelProuctLicens.Description = product.Description;
                    var license = new License()
                    {
                        LicenseID = product.LicenseID,
                        MaxLicenseCount = product.MaxLicenseCount,
                        IsActive = product.IsActive,
                        ExpiryDate = product.ExpiryDate,
                        Price = product.Price
                    };
                    lstLicense.Add(license);
                }
                modelProuctLicens.Licenses = lstLicense;
                rsData.Add(modelProuctLicens);
            }
            return new ServiceResponse(true, "Lấy dữ liệu thành công", rsData);
        }
    }
}
