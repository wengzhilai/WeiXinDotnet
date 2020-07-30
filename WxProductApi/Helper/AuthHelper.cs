using Microsoft.IdentityModel.Tokens;
using Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper
{
    public class AuthHelper
    {
        public static string GenerateToken(SysUserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //chave secreta
            var key = Encoding.ASCII.GetBytes("ZGVtby1hcGktand0");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.name.ToString()),
                    new Claim(ClaimTypes.Role, RoleFactory(user.roleIdList))
                }),
                Expires = DateTime.UtcNow.AddHours(10),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static string RoleFactory(List<int> roleNumber)
        {
            return "Director";
        }
    }
}