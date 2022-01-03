using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Projet2.Models.Boutique;

namespace Projet2.Models.Mails
{
	public class MailService : IMailService
	{
		private readonly IMailConfiguration _emailConfiguration;

		public MailService(IMailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public List<EMailMessage> ReceiveEmail(int maxCount = 10)
		{
			throw new NotImplementedException();
		}

		public void Send( String Name, String Address)
		{
			var message = new MimeMessage();
			EMailMessage emailMessage = new EMailMessage();

			emailMessage.Content = "Bonjour \n" +
                " Nous vous remercions de votre confiance \n" +
                " ci dessous le résumé de votre commande ";
			emailMessage.Subject = "Commande chez Amaporte";
			message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(Name, Address)));
			message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress("Administrateurs du site", "Amaporte@gmail.com")));

			message.Subject = emailMessage.Subject;
			//We will say we are sending HTML. But there are options for plaintext etc. 
			message.Body = new TextPart(TextFormat.Html)
			{
				Text = emailMessage.Content,

			};
			BodyBuilder bodyBuilder = new BodyBuilder();


			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				//The last parameter here is to use SSL (Which you should!)
				emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

				//Remove any OAuth functionality as we won't be using it. 
				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

				emailClient.Send(message);

				emailClient.Disconnect(true);
			}

		}
	}
}
