﻿using Microsoft.IdentityModel.Tokens;

namespace UltraSystem.Core.Model.Core
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public string SecretKey { get; set; }
        public TimeSpan TokenExpiresAfter { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan Expiration { get; set; }
        public DateTimeOffset NotBefore { get; set; }
        public TimeSpan ExpiresRefreshToken { get; set; }
        public string JwtId { get; set; }
    }
}
