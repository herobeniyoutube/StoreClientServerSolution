using StoreServer.Entities;
using StoreServer.Models.EFCoreEntitiesCopies;

namespace StoreServer.Models
{
    /// <summary>
    /// Model of jwt Token which client gets on succesful autorization
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Constructs new jwt token
        /// </summary>
        /// <param name="encodedJwt"></param>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public Token(string encodedJwt, User user, string role)
        {
            AccessToken = encodedJwt;
            UserLogin = user.UserLogin;
            UserName = user.UserName;
            Text = "success";
            Id = user.Id;
            Role = role;
        }
        public string AccessToken {  get; set; }
        public string UserLogin {  get; set; }
        public string UserName {  get; set; }
        public string Text {  get; set; }
        public int Id {  get; set; }
        public string Role {  get; set; }

    }
}
