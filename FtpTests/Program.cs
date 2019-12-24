using Limilabs.FTP.Client;
using Rebex.Net;
using System;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using Ftp = Limilabs.FTP.Client.Ftp;

namespace FtpTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string IP = "10.175.128.225";
            string UserName = "invoicelog";
            string Password = "hECbSYw7nq";
            string directory = "/ElectronicBilling/AtilioTeste/";

            try
            {
                using (Limilabs.FTP.Client.Ftp client = new Ftp())
                {
                    client.ServerCertificateValidate += ValidateCertificate;
                    client.SSLConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
                    client.ConnectSSL(IP);     // or ConnectSSL for SSL
                    client.Login(UserName, Password);
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
            finally
            {
                //TEST another LIB
                Rebex.Licensing.Key = "==AOprPa8kezwq++7Gv/FSJ8IwylrPtv1Wb2ebvS3dfBAA==";
                try
                {
                    using (var client = new Rebex.Net.Ftp())
                    {
                        // connect and log in
                        client.ValidatingCertificate += Client_ValidateCertificate;
                        client.Connect(IP, Rebex.Net.SslMode.Implicit);
                        client.Login(UserName, Password);
                        client.SecureTransfers = true;
                        // download a file
                        client.Download(directory, GetTemporaryDirectory());
                        Console.Write("SUCCESS");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void Client_ValidateCertificate(object sender, SslCertificateValidationEventArgs e)
        {
            e.Accept();
        }

        public static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
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
