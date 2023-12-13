using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.RequestModel;

namespace UltraSystem.Core.Interface
{
    public interface IRolePermisstionRepository:IGenericRepository<RolePermisstion>
    {
        Task<List<RolePermisstion>> GetRolePermisstionsByUserID(string UserID);
        Task<List<RolePermisstion>> GetAllRolePermisstions();
        Task<object> InsertRolePermisstion(RolePermisstionRequestModel model);
    }
}
