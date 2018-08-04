using System.Configuration;

namespace EBillAppTest.Configuration
{
    public class Settings
    {
        private const string SMTPSERVER = "SMTPServer";
        private const string PORT = "Port";
        private const string USERID = "UserId";
        private const string PASSWORD = "Password";
        private const string FACEBOOKCONNECT = "FacebookConnect";
        private const string INSTAGRAMCONNECT = "InstagramConnect";

        public static string SMTPServer
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(SMTPServer)];
            }
        }

        public static string Port
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(Port)];
            }
        }

        public static string UserId
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(UserId)];
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(Password)];
            }
        }

        public static string FacebookConnect
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(FacebookConnect)];
            }
        }

        public static string InstagramConnect
        {
            get
            {
                return ConfigurationManager.AppSettings[nameof(InstagramConnect)];
            }
        }
    }
}