using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StoreServer.Models
{
    /// <summary>
    /// Model that has configuration for jwt Token construction
    /// </summary>
    public class AuthentificationOptions
    {
        public const string ISSUER = "StoreServer";  
        public const string AUDIENCE = "StoreClient";  
        const string KEY = "sadfphsw98gsw089gyf089w";   
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
