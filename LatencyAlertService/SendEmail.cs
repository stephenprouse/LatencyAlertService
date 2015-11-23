using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

class SendEmail
{
    public void SendEmailMsg(string mailServer, string mailSender, string mailRecipient, string mailSubject, string mailBody)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(mailServer);

            mail.From = new MailAddress(mailSender);
            mail.To.Add(mailRecipient);
            mail.Subject = mailSubject;
            mail.Body = mailBody;

            SmtpServer.Port = 25;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
            //SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
        catch (Exception ex)
        {

        }
    }
}