using System.Net.Mail;
using System.Net;

namespace FMP_DISPATCH_API.Services.Emails
{
    public class EmailSender : IEmailSender
    {
        public void SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                string? mail = "enmanuel02bnunez@gmail.com";
                string? pwd = "odad vors enbs nuot";
                using (MailMessage email = new MailMessage())
                {
                    email.From = new MailAddress(mail);
                    email.To.Add(to);
                    email.Subject = subject;
                    email.Body = message;
                    email.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(mail, pwd);
                        smtp.EnableSsl = true;
                        smtp.Send(email);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }

}


