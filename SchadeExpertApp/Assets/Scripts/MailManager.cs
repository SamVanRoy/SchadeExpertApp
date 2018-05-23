using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
#if NETFX_CORE
using Windows.ApplicationModel.Email;
using Windows.Storage;
#endif
//using MimeKit;
//using MailKit.Net.Smtp;
//using MailKit;

public class MailManager : MonoBehaviour
{

    public static void feestje()
    {
//        Debug.Log("feestje");
//        var message = new MimeMessage();
//        message.From.Add(new MailboxAddress("Joey Tribbiani", "saxo4sam@gmail.com"));
//        message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "saxo4sam@gmail.com"));
//        message.Subject = "How you doin'?";

//        message.Body = new TextPart("plain")
//        {
//            Text = @"Hey Chandler,

//I just wanted to let you know that Monica and I were going to go play some paintball, you in?

//-- Joey"
//        };

//        using (var client = new SmtpClient())
//        {
//            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
//            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

//            client.Connect("smtp.gmail.com", 587, false);

//            // Note: only needed if the SMTP server requires authentication
//            client.Authenticate("saxo4sam@gmail.com", "Smurfenijsje0205");

//            client.Send(message);
//            client.Disconnect(true);
//        }
    }
}
