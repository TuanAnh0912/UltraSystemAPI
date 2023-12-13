using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Interface
{
    public interface IAuthenService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<string> GetTokenByRefreshToken(string rfToken);
    }
}
