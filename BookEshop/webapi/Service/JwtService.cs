using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;

namespace webapi.Service
{
    public class JwtService : IJwtService
    {
        public string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //toto mi vytvori data v HEADER (key) -> dala som ho encode ako bytes lebo do credentials mi pojdu bytes
            var key = Encoding.ASCII.GetBytes("veryverysecret...");
            //Toto mi posle informacie do PAYLOAD: DATA casti ako napriklad role a username
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.UserName)
            });
            //Toto mi vytvori credentials - poslem si tam key, ktory som si vytvorila a druhy parameter je algoritmus, ktory pouzijem na vytvorenie credentials - HmacSha256 sa casto pouziva - preto ho mam
            //je to teda cast tokenu verify signature (uplne posledna)
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity, //Subject je moja identity
                Expires = DateTime.Now.AddDays(1), //za aku dobu token expirne - kazdy token musi mat expiry time - tento token je validny od doby jeho vytvorenia po dalsi den
                SigningCredentials = credentials
            };

            //vytvorenie tokenu
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            //toto vrati encrypted token
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
