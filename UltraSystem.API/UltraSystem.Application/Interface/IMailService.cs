using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Application.Interface
{
    public interface IMailService
    {
        bool SendMail(string to, string subject, string html, string from = null);
    }
}
