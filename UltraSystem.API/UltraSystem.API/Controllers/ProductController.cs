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
    public class ProductController : BaseServiceController<Products>
    {
        readonly IProductService _productService;
        public ProductController(IProductService baseService, IProductService productService) : base(baseService)
        {
            this.currentType = typeof(Products);
            _productService = productService;
        }
    }
}