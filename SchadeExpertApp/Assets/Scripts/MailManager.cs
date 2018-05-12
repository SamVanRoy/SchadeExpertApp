using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
#if NETFX_CORE
using Windows.ApplicationModel.Email;
using Windows.Storage;
#endif

public class MailManager : MonoBehaviour {

    public static void grmbl(List<string> currentProjectFilesPaths)
    {
        string email = "saxo4sam@gmail.com";
        string Subject = "About Sampie";
        string Body = "\n\nSent from the Hololens App";
        Debug.Log(currentProjectFilesPaths[0]);
        Application.OpenURL("mailto:" + email + "?subject=" + Subject + "&body=" + Body + "&Attachment=" + currentProjectFilesPaths[0]);

        //MAPI mapi = new MAPI();
        ////mapi.AddAttachment("c:\\temp\\file1.txt");
        ////mapi.AddAttachment("c:\\temp\\file2.txt");
        ////mapi.AddRecipientTo("person1@somewhere.com");
        //mapi.AddRecipientTo("saxo4sam@gmail.com");
        //mapi.SendMailPopup("testing", "body text");
    }

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

    //public void SendMail()
    //{
    //    MailMessage mail = new MailMessage();

    //    mail.From = new MailAddress("saxo4sam@gmail.com");
    //    mail.To.Add("saxo4sam@gmail.com");
    //    mail.Subject = "Test Mail";
    //    mail.Body = "This is for testing SMTP mail from GMAIL";

    //    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
    //    smtpServer.Port = 587;
    //    smtpServer.Credentials = new System.Net.NetworkCredential("saxo4sam@gmail.com", "Smurfenijsje0205") as ICredentialsByHost;
    //    smtpServer.EnableSsl = true;
    //    ServicePointManager.ServerCertificateValidationCallback =
    //        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //        { return true; };
    //    smtpServer.Send(mail);
    //    Debug.Log("success");
    //}


    //    public static async Task SendEmail()
    //    {
    //        Debug.Log("sending an email!!!!");
    //#if NETFX_CORE
    //        //EmailMessage emailMessage = new EmailMessage();
    //        Debug.Log("sending an email1");
    //        //emailMessage.To.Add(new EmailRecipient("saxo4sam@gmail.com"));
    //        Debug.Log("sending an email!!2");
    //        //string messageBody = "Hello World";
    //        Debug.Log("sending an email!!!!3");
    //        //emailMessage.Body = messageBody;
    //        //StorageFolder MyFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
    //        //StorageFile attachmentFile = await MyFolder.GetFileAsync("MyTestFile.txt");
    //        //if (attachmentFile != null)
    //        //{
    //        //    var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(attachmentFile);
    //        //    var attachment = new Windows.ApplicationModel.Email.EmailAttachment(
    //        //             attachmentFile.Name,
    //        //             stream);
    //        //    emailMessage.Attachments.Add(attachment);
    //        //}
    //        Debug.Log("sending an email!!!4");
    //        //await EmailManager.ShowComposeNewEmailAsync(emailMessage);
    //        Debug.Log("sending an email!!!!5");

    //        EmailMessage email = new EmailMessage();
    //        Debug.Log("sending an email10");
    //        email.To.Add(new EmailRecipient("samvanroy8@hotmail.com"));
    //        Debug.Log("sending an email11");
    //        email.Subject = "Blog post by fdsfsdl";
    //        EmailManager.ShowComposeNewEmailAsync(email);
    //        Debug.Log("sending an email12");
    //#endif
    //}
}
        