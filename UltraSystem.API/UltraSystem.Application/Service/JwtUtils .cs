﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Service
{
    public class JwtUtils : IJwtUtils
    {
        private JwtIssuerOptions _jwtIssuerOptions;
        private IUserRepository _userRepository;
        public JwtUtils(IOptions<JwtIssuerOptions> jwtIssuerOptions, IUserRepository userRepository)
        {
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _userRepository = userRepository;

        }
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? ""));
            claims.Add(new Claim("UserID", user.UserID.ToString()));
            claims.Add(new Claim("Email", user.Email?.ToString() ?? ""));
            claims.Add(new Claim("UserName", user.UserName?.ToString() ?? ""));
            claims.Add(new Claim("RoleName", "User"));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtIssuerOptions.SecretKey));

            var tokenRender = new JwtSecurityToken(
            issuer: _jwtIssuerOptions.Issuer,
             audience: _jwtIssuerOptions.Audience,
             expires: DateTime.UtcNow.Add(_jwtIssuerOptions.TokenExpiresAfter),
             claims: claims,
             signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(tokenRender);
        }

        public async Task<string> GenerateRefreshToken()
        {
            return await GetUniQueToken();
        }
        public async Task<string> GetUniQueToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenIsUnique = await _userRepository.GetUserByRefreshToken(token);
            if (tokenIsUnique != null)
                return await GetUniQueToken();
            return token;
        }
        public string GenerateTokenWithExpiry()
        {
            byte[] tokenBytes = new byte[32]; // Adjust the length of the token as needed
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenBytes);
            }

            string token = Convert.ToBase64String(tokenBytes);
            DateTime expiryDateTime = DateTime.UtcNow.AddMinutes(10);
            string expiryString = expiryDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

            return $"{token}:{expiryString}";
        }

        public string GenerateResetLink()
        {
            string baseUrl = "https://example.com/reset-password";
            var tokenWithExpiry = GenerateTokenWithExpiry();
            StringBuilder sb = new StringBuilder(baseUrl);
            sb.Append("?token=");
            sb.Append(Uri.EscapeDataString(tokenWithExpiry));
            return sb.ToString();
        }
    }
}
