using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UltraSystem.Core.Interface;

namespace UltraSystem.API.Middleware.TokenHandle
{
    public class AuthHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        public AuthHandleMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                ValidateToken(context);
            }
            catch (Exception)
            {

                throw;
            }
            await _next(context);
        }
        private void ValidateToken(HttpContext context)
        {
            try
            {
                var claimProvider = _serviceProvider.GetRequiredService<IClaimProvider>();
                if (context.Request == null || context.Request.Headers == null || !context.Request.Headers.ContainsKey("Authorization"))
                {
                    claimProvider.SetUserID(Guid.Empty.ToString());
                    return;
                }
                var authenToken = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authenToken))
                {
                    authenToken = authenToken.ToString().Split(" ")[1];
                    var handleToken = new JwtSecurityTokenHandler();
                    var jsonToken = handleToken.ReadJwtToken(authenToken);
                    if (jsonToken != null)
                    {
                        var claims = jsonToken.Claims;
                        foreach (var claim in claims)
                        {
                            string claimType = claim.Type;
                            string claimValue = claim.Value;
                            if (claimType == "UserID")
                            {
                                claimProvider.SetUserID(claimValue);
                            }
                            else if(claimType == "Email")
                            {
                                claimProvider.SetEmail(claimValue);
                            }else if (claimType == "UserName")
                            {
                                claimProvider.SetUserName(claimValue);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
