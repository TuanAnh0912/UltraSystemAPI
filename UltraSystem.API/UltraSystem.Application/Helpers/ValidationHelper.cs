using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UltraSystem.Application.Helpers
{
    public class ValidationHelper
    {
        public static bool IsValid(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com.vn|com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
        public static string IsValidEmail(string email)
        {
            string rsValidateEmail = ValidatEmpty(email, "Email");
            if (!string.IsNullOrEmpty(rsValidateEmail))
            {
                return rsValidateEmail;
            }
            if (!IsValid(email))
            {
                return "Email không đúng định dạng";
            }
            return "";
        }
        public static string ValidatEmpty(string value, string field)
        {
            if (string.IsNullOrEmpty(value))
            {
                return $"{field} không được để trống";
            }
            return "";
        }
    }
}
