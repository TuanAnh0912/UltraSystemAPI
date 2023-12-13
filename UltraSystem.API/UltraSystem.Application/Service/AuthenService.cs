using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UltraSystem.Application.Helpers;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Repositories;

namespace UltraSystem.Application.Service
{
    public class AuthenService : IAuthenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private JwtIssuerOptions _jwtIssuerOptions;
        public AuthenService(IUserRepository userRepository, IJwtUtils jwtUtils, IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            _userRepository = userRepository;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _jwtUtils = jwtUtils;
        }
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            password = AuthenHelpers.HashPassword(username + password);
            var rsCheckLogin = await _userRepository.CheckLogin(username, password);
            //todo: mesage sai: trường hợp tài khoản tồn tại rồi vẫn trả về "Tài khoản không tồn tại
            if (rsCheckLogin == null)
            {
                return new LoginResponse(false, "Tài khoản hoặc mật khẩu không đúng");
            }

            var token = _jwtUtils.GenerateJwtToken(rsCheckLogin);
            var refreshToken = await _jwtUtils.GenerateRefreshToken();
            rsCheckLogin.RefreshToken = refreshToken;
            var condition = $"UserID = '{rsCheckLogin.UserID}'";
            var resUpdate = await _userRepository.UpdateCustomColumn(rsCheckLogin, new List<string>() { "RefreshToken" }, condition);
            if (resUpdate)
            {
                return new LoginResponse(true, "Đăng nhập thành công", refreshToken, token);
            }
            return new LoginResponse(false, "Tài khoản hoặc mật khẩu không đúng");
        }
        public async Task<string> GetTokenByRefreshToken(string rfToken)
        {
            var userByRefreshToken = await _userRepository.GetUserByRefreshToken(rfToken);
            if (userByRefreshToken == null)
            {
                return "";
            }
            return _jwtUtils.GenerateJwtToken(userByRefreshToken) ?? "";
        }
    }
}
