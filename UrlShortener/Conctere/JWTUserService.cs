using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Conctere
{
    public class JWTUserService : IJWTUserService
    {
        private readonly TokenModel tokenModel;
        private readonly IDatabaseRepository _context;

        public JWTUserService(IOptions<TokenModel> appSettings, IDatabaseRepository context)
        {
            tokenModel = appSettings.Value;
            _context = context;
        }

        public string Authenticate(UserModel model)
        {
            var user = _context.Users.GetRecords().Where(x => x.Login == model.Login && x.Password == model.Password).FirstOrDefault();

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);


            return token;
        }

        public UserModel GetById(int id)
        {
            return _context.Users.GetRecord(id);
        }

        // helper methods

        private string generateJwtToken(UserModel user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenModel.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
