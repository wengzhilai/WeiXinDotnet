using Microsoft.IdentityModel.Tokens;
using Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper
{
    /// <summary>
    /// 授权
    /// </summary>
    public class AuthHelper
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateToken(SysUserEntity user)
        {
            //chave secreta
            var key =Encoding.ASCII.GetBytes( WxProductApi.Global.appConfig.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.name.ToString()),
                    new Claim(ClaimTypes.Role, RoleFactory(user.roleIdList))
                }),
                Expires = DateTime.UtcNow.AddHours(10),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string RoleFactory(List<int> roleNumber)
        {
            return "Director";
        }
    }
}