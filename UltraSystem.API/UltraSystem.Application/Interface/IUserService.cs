using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Model.RequestModel;

namespace UltraSystem.Application.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<ServiceResponse> GetUserLicenses();
        Task<ServiceResponse> GetAllRole();
        Task<ServiceResponse> InsertRolePermisstion(RolePermisstionRequestModel model);
        Task<ServiceResponse> SendMail();
        Task<ServiceResponse> ResetPassword(string newPassword);
    }
}
