using System;
using MailKit.Net.Smtp;
using MimeKit;
namespace Projet2.Models.Mails
{
    public class CommandeMail
    {
        MimeMessage Message = new MimeMessage();

        MailboxAddress from = new MailboxAddress("Admin",
        "admin@example.com");

        MailboxAddress to = new MailboxAddress("User",
        "user@example.com");
    
    }
}
