using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            string? signingKey = _config["JWT:SigningKey"];
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }



        public string CreateToken(AppUser appUser)
        {
            // Create the Claims that We Wish to Include
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName)
            };

            // Create the Siging Credentials
            var signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            // Create the Token Description
            // (The Object Representation of the Token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signingCredentials,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            // Create the Handler which will Create the Actual Token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create the Actual Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Don't Send the Actual Token\
            // Send it as a String
            return tokenHandler.WriteToken(token);
        }
    }
}
