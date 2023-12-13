using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Model;

namespace UltraSystem.Application.Service
{
    public class MailService : IMailService
    {
        private readonly MailAppSetting _mailAppSetting;
        public MailService(IOptions<MailAppSetting> options)
        {
            _mailAppSetting = options.Value;
        }
        public bool SendMail(string to, string subject, string html, string from = null)
        {
            try
            {
                var email = new MailMessage()
                {
                    From = new MailAddress(from ?? _mailAppSetting.MailFrom),
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                };
                email.To.Add(to);
                var smtpClient = new SmtpClient(_mailAppSetting.SmtpHost)
                {
                    Port = _mailAppSetting.SmtpPort,
                    Credentials = new NetworkCredential(_mailAppSetting.SmtpUser, _mailAppSetting.SmtpPass),
                    EnableSsl = true,
                };
                smtpClient.Send(email);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
