using System.Security.Cryptography;
using System.Text;

namespace BaseProjectApp.API.Authentication
{
    public class RSA
    {
        private readonly IHostEnvironment environment;

        public RSA(IHostEnvironment environment)
        {
            this.environment = environment;
        }

        public string Decrypt(string strText)
        {
            string fullFilePath = environment.ContentRootPath + "\\Keys\\private.xml";
            var privateKey = File.ReadAllText(fullFilePath);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var base64Encrypted = strText;
                    base64Encrypted = base64Encrypted.Replace(" ", "+");
                    rsa.FromXmlString(privateKey);

                    var resultBytes = Convert.FromBase64String(base64Encrypted);
                    var decryptedBytes = rsa.Decrypt(resultBytes, false);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

    }
}
