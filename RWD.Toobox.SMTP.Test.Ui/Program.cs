using System;
using Microsoft.Extensions.Configuration;
using RWD.Toolbox.SMTP;

namespace RWD.Toobox.SMTP.Test.Ui
{
    class Program
    {

        private static IConfigurationRoot Configuration { get; set; }

        const string SMTP_IpAddress = "SMTPConfig:IP";
        const string SMTP_PortNumber = "SMTPConfig:Port";
        const string SMTP_SSL = "SMTPConfig:SSL";
        const string SMTP_UserName = "SMTPConfig:UserName";
        const string SMTP_UserPassword = "SMTPConfig:UserPassword";
       

        static void Main(string[] args)
        {
            try
            {

                BootstrapConfiguration();
                var ip = Configuration.GetSection(SMTP_IpAddress).Value;
                var port = Configuration.GetSection(SMTP_PortNumber).Value;
                var useSsl = Configuration.GetSection(SMTP_SSL).Value;
                var userName = Configuration.GetSection(SMTP_UserName).Value;
                var userPassword = Configuration.GetSection(SMTP_UserPassword).Value;

                Console.WriteLine($"The value of ip is {ip}");
                Console.WriteLine($"The value of port is {port}");
                Console.WriteLine($"The value of SSL is {useSsl}");
                Console.WriteLine($"The value of username is {userName}");
                Console.WriteLine($"The value of userPassword is {userPassword}");

                IEmailAgent emailAgent = new EmailAgent
                {
                    SMTP_IP = ip.ToString(),
                    SMTP_Port = int.Parse(port),
                    SMTP_Ssl = bool.Parse(useSsl),
                    SMTP_User = userName,
                    SMTP_Password = userPassword
                };

                string[] attachments = new string[] { @"C:\Users\ka8kgj\Desktop\freeze.txt" };
                string fromEmail = "jstevens@realworlddevelopers.com";
                string fromName = "jstevens";
                string toEmail = "ka8kgj@outlook.com";
                string subjectLine = "This is a test Email";
                string emailBody = "<h1>Testing</h1><strong>This is a test</strong>";

                emailAgent.SendEmail(fromEmail, fromName, toEmail, subjectLine, emailBody, true, attachments);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Test complete. Hit any key to end.");
            Console.ReadKey(true);

        }


        private static void BootstrapConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            Configuration = builder.Build();
        }

    }

}
