using System.Data;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.ResponseModel;

namespace UltraSystem.Core.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> CheckLogin(string username, string password);
        Task<User> CheckByUserNameAndEmail(string userName, string email);
        Task<List<UserLicenseFromModel>> GetUserAndLicensOwner();
        Task<bool> UpdateAcountBalance(decimal acountBalance, Guid userID, IDbTransaction transaction = null);
        Task<User?> GetUserByRefreshToken(string resfreshToken);
    }
}
