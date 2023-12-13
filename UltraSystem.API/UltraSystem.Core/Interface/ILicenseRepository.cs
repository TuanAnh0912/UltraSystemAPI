using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;

namespace UltraSystem.Core.Interface
{
    public interface ILicenseRepository:IGenericRepository<License>
    {
        Task<List<License>> GetLicensesByUserID(Guid userID);
        Task<List<License>> GetLicenseByProductID(int productID);
    }
}
