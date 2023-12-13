using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.ResponseModel;

namespace UltraSystem.Core.Interface
{
    public interface IProductRepository:IGenericRepository<Products>
    {
        Task<IEnumerable<ProductFromDbModel>> GetAllProductLicens();
    }
}
