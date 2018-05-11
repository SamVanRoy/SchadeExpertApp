using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MailManager : MonoBehaviour {
    //public void lol()
    //{
    //    using (var client = new Pop3Client())
    //    {
    //        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
    //        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

    //        client.Connect("pop.friends.com", 110, false);

    //        client.Authenticate("joey", "password");

    //        for (int i = 0; i < client.Count; i++)
    //        {
    //            var message = client.GetMessage(i);
    //            Console.WriteLine("Subject: {0}", message.Subject);
    //        }

    //        client.Disconnect(true);
    //    }
    //}

    public void SendMail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("saxo4sam@gmail.com");
        mail.To.Add("saxo4sam@gmail.com");
        mail.Subject = "Test Mail";
        mail.Body = "This is for testing SMTP mail from GMAIL";

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("saxo4sam@gmail.com", "Smurfenijsje0205") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
    }
}
        