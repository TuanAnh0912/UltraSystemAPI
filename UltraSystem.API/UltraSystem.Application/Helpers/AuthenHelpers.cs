using Azure;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
namespace UltraSystem.Application.Helpers
{
    public  class AuthenHelpers
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
