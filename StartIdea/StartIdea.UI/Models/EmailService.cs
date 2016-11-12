using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace StartIdea.UI.Models
{
    public static class EmailService
    {
        private static string Host = ConfigurationManager.AppSettings.Get("Host");
        private static string Remetente = ConfigurationManager.AppSettings.Get("Remetente");
        private static int PortaSMTP = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PortaSMTP"));
        private static string Senha = ConfigurationManager.AppSettings.Get("Senha");

        public static bool EnviarEmail(string Assunto, string Conteudo, string Destinatario)
        {
            try
            {
                MailMessage mail = new MailMessage(Remetente, Destinatario, Assunto, Conteudo);
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(Host, PortaSMTP);
                smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(Remetente, Senha);
                smtp.Timeout = 20000;
                smtp.Send(mail);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}