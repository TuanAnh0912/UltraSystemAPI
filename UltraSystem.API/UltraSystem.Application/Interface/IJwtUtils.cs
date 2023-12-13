using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model;

namespace UltraSystem.Application.Interface
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public Task<string> GenerateRefreshToken();
        string GenerateResetLink();
    }
}
