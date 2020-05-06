using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ang_net.services
{
    public static class SendGridAPI
    {
      public  static async Task <bool> Execute(string UserEmail ,string UserName ,string plainTextContent,string htmlContent,string subject)
        {
            var apiKey = "SG.kORmaNrERquasX72Zonqyg.Gc7ywxnmBbDrYoaoUigxyKx14D71sYqN7yujavsGOuc";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Administration");
            
            var to = new EmailAddress(UserEmail, UserName);       
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return await Task.FromResult(true);
        }
    }
}
