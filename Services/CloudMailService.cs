using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];

        private string _mailFrom = Startup.Configuration["mailSettings:mailAddressAddress"];
    
        public void Send(string subject, string message) 
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with CloudMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Mesage: {message}");
        }
    }
}