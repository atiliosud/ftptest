using Limilabs.FTP.Client;
using System;
using System.Configuration;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;

namespace FtpTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string IP = "10.175.128.225";
                string UserName = "invoicelog";
                string Password = "hECbSYw7nq";

                using (Ftp client = new Ftp())
                {
                    client.ServerCertificateValidate += ValidateCertificate;
                    client.SSLConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
                    client.ConnectSSL(IP);     // or ConnectSSL for SSL
                    client.Login(UserName, Password);
                    string directory = "/ElectronicBilling/AtilioTeste/";

                    byte[] bytes = client.Download(directory + "CustomerAlphavilleNovo.Tab");
                    string report = Encoding.Default.GetString(bytes);

                    Console.Write(report);

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ValidateCertificate(
         object sender,
         ServerCertificateValidateEventArgs e)
        {
            const SslPolicyErrors ignoredErrors =
                SslPolicyErrors.RemoteCertificateChainErrors |
                SslPolicyErrors.RemoteCertificateNameMismatch;

            if ((e.SslPolicyErrors & ~ignoredErrors) == SslPolicyErrors.None)
            {
                e.IsValid = true;
                return;
            }
            e.IsValid = false;
        }
    }
}
