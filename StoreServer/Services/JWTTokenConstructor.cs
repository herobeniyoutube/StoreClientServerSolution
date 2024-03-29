using Microsoft.IdentityModel.Tokens;
using StoreServer.Entities;
using StoreServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StoreServer.Services
{
    /// <summary>
    /// Service for creating jwt token
    /// </summary>
    public class JWTTokenConstructor
    {
        private Role adminRole = new Role("admin");
        private Role userRole = new Role("user");
        /// <summary>
        /// gets new jwt token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token GetToken(UserEntity user)
        {

            var role = userRole;
            if (user.UserLogin == "admin") role = adminRole;

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserLogin),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
    };

            var jwt = new JwtSecurityToken(
                   issuer: AuthentificationOptions.ISSUER,
                   audience: AuthentificationOptions.AUDIENCE,
                   claims: claims,
                   expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                   signingCredentials: new SigningCredentials(AuthentificationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            Token token = new Token(encodedJwt, user, role.Name);
            
            return token;
        }
    }
}
