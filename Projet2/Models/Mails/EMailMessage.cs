using System;
using System.Collections.Generic;

namespace Projet2.Models.Mails
{
    public class EMailMessage
    {
		public List<EMailAdresse> ToAddresses { get; set; }
		public List<EMailAdresse> FromAddresses { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }

		public EMailMessage()
			{
				ToAddresses = new List<EMailAdresse>();
				FromAddresses = new List<EMailAdresse>();
			}

			
	}
	
}
