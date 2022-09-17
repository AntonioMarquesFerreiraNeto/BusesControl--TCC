using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace BusesControl.Helper {
    public class Email : IEmail {

        private readonly IConfiguration _configuration;

        public Email(IConfiguration configuration) {
            _configuration = configuration;
        }
        public bool Enviar(string email, string tema, string msg) {
            try {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string userName = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                MailMessage mail = new MailMessage() {
                    From = new MailAddress(userName, nome)
                };

                mail.To.Add(email);
                mail.Subject = tema;
                mail.Body = msg;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, porta)) {
                    smtp.Credentials = new NetworkCredential(userName, senha);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                    return true;
                }

            }
            catch (Exception erro) {
                return false;
            }
        }
    }
}
