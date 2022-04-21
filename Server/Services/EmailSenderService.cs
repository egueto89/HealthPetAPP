using HealthPetAPP.Shared;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace HealthPetAPP.Server.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSetttings _smtpSetttings;

        public EmailSenderService(IOptions<SmtpSetttings> smtpSetttings)
        {
            _smtpSetttings = smtpSetttings.Value;
        }
        public async Task<int> SendEmailAsync(DatosCorreo datosCorreo)
        {
            int result = 0;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSetttings.SenderName,_smtpSetttings.SenderEmail));
                message.To.Add(new MailboxAddress("", datosCorreo.Correo));
                message.Subject = "Confirmación de Cita";
                message.Body = new TextPart("html") { Text = datosCorreo.CuerpoCorreo };

                using (var resource = new SmtpClient())
                {
                    resource.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await resource.ConnectAsync(_smtpSetttings.Server,Convert.ToInt32(_smtpSetttings.Port),false);
                    await resource.AuthenticateAsync(_smtpSetttings.UserName, _smtpSetttings.Password);
                    await resource.SendAsync(message);
                    await resource.DisconnectAsync(true);
                 }
            }
            catch (Exception ex)
            {
                // TO DO
                result = 1;
            }

            return result;
        }
    }
}
