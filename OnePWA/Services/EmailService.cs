using Microsoft.Extensions.Configuration;
using MimeKit;
using OnePWA.Helpers;
using OnePWA.Models.Entities;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace OnePWA.Services
{
    public class EmailService:IEmailService
    {
        public EmailService(OnecgdbContext context, IConfiguration configuration)
        {
            Context=context;
            Configuration=configuration;
        }

        public OnecgdbContext Context { get; }
        public IConfiguration Configuration { get; }

        public async Task SendEmailAsync(string toEmail)
        {
            var smtpSettings = Configuration.GetSection("SmtpSettings");
            var server = smtpSettings["Server"];
            var port = int.Parse(smtpSettings["Port"]!);
            var senderEmail = smtpSettings["SenderEmail"];
            var senderName = smtpSettings["SenderName"];
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            var datos = Context.Users.FirstOrDefault(x=>x.Email == toEmail);
            string newPassword = ContraseñaHelper.GenerarContraseña(datos);

            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Restablecer Contraseña",
                    Body = $"<h1>Tu nueva contrasela es:</h1> <h2>{newPassword}</h2>",
                    IsBodyHtml = true
                };
                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
        }


        //public void SendEmailAsync(string email)
        //{
        //    var datos = Context.Users.FirstOrDefault(x => x.Email==email);


        //    #region "email"
        //    var mail = "unotegame@gmail.com";
        //    var password = "hjwe dsnx stfk mfuw";
        //    #endregion
        //    //generar password
        //    var subject = "Restablecer Contraseña";
        //    var msj = $"<h1>CODIGO</h1>";



        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("ONE GAME", mail));
        //    message.To.Add(new MailboxAddress("Usuario", email));
        //    message.Subject= subject;
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = msj
        //    };

        //    using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
        //    {

        //        client.Port=587;
        //        client.EnableSsl = true;

        //        client.DeliveryMethod=SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        System.Net.NetworkCredential credential = new NetworkCredential(mail, password);
        //        client.Credentials = credential;


        //        MailMessage msg = new MailMessage(mail, email);
        //        msg.Subject = subject;
        //        msg.Body = msj;
        //        msg.IsBodyHtml = true;

        //        client.Send(msg);



        //    }
        //}
    }
}
