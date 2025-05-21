using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace saga.Properties
{
    /// <summary>
    /// Represents the signing configuration for generating tokens using an RSA key.
    /// </summary>
    public class SigningConfiguration : ISigningConfiguration
    {
        /// <inheritdoc />
        public RsaSecurityKey Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningConfiguration"/> class with the specified RSA key.
        /// </summary>
        /// <param name="key">The RSA key XML string.</param>
        public SigningConfiguration(string key)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(key);
                    this.Key = new RsaSecurityKey(rsa.ExportParameters(true));
                }
                catch (CryptographicException)
                {
                    this.Key = new RsaSecurityKey(rsa.ExportParameters(true));
                }
            }
        }
    }
}
