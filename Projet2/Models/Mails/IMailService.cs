using System;
using System.Collections.Generic;

namespace Projet2.Models.Mails
{
    public interface IMailService
    {
        void Send(String Name, String Address);
        List<EMailMessage> ReceiveEmail(int maxCount = 10);
    }
}
