using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;

namespace SmsSender
{
    class Program
    {
        static void Main(string[] args)
        {
            // Yapılandırma dosyasını yükleyin
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // TwilioSettings bölümündeki bilgilere erişin
            string accountSid = configuration["TwilioSettings:AccountSid"];
            string authToken = configuration["TwilioSettings:AuthToken"];
            string twilioNumber = configuration["TwilioSettings:TwilioNumber"];

            try
            {
                // Twilio Client'ı yapılandırın
                TwilioClient.Init(accountSid, authToken);

                // SMS alıcı numarası ve mesaj içeriğini girin
                Console.Write("SMS gönderilecek numarayı girin (örn. +11111111): ");
                string recipientNumber = Console.ReadLine();

                Console.Write("SMS içeriğini girin: ");
                string messageBody = Console.ReadLine();

                // SMS gönderin
                var messageOptions = new CreateMessageOptions(new PhoneNumber(recipientNumber))
                {
                    From = new PhoneNumber(twilioNumber),
                    Body = messageBody
                };

                var message = MessageResource.Create(messageOptions);
                Console.WriteLine($"SMS gönderildi. SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS gönderme hatası: {ex.Message}");
            }
        }
    }
}
