using Microsoft.AspNetCore.Mvc;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenService _authenService;

        public AuthenticateController(IAuthenService authenService)
        {
            _authenService = authenService;
        }
        [HttpPost("login")]
        public async Task<ServiceResponse> CheckLogin([FromBody] LoginRequest data)
        {
            var rsLogin =  await _authenService.LoginAsync(data.Username, data.password);
            if (!rsLogin.Success)
            {
                return new ServiceResponse(rsLogin.Success, rsLogin.Message ?? "");
            }
            setTokenCookie(rsLogin.RefreshToken);
            return new ServiceResponse(rsLogin.Success, rsLogin.Message ?? "", new
            {
                access_token = rsLogin.ToKen
            });
        }
        [HttpGet("refresh-token")]
        public async Task<ServiceResponse> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                return new ServiceResponse(false, "RefreshToken đã hết hạn");
            }
            var getToken = await _authenService.GetTokenByRefreshToken(refreshToken);
            if (string.IsNullOrEmpty(getToken))
            {
                return new ServiceResponse(false, "Lấy token mới không thành công");
            }
            return new ServiceResponse(true, "Lấy token mới thành công", getToken);
        }
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse("00:20:00"))
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}