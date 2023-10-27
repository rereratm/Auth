using AppDAL.Entity;
using AppDAL.LoginSecurity.Encryption;
using AppDAL.LoginSecurity.Entity;
using AppDAL.LoginSecurity.Extension;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.LoginSecurity.Helper
{
    public class TokenHelper : ITokenHelper
    {

        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        //IOptions => Microsoft.Extensions.Options
        public TokenHelper(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }

        public AccessToken CreateToken(Kullanici kullanici, IEnumerable<Role> userRoles)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwtSecurityToken = CreateJwtSecurityToken(_tokenOptions, kullanici, signingCredentials, userRoles);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Kullanici kullanici,
           SigningCredentials signingCredentials, IEnumerable<Role> roles)
        {
            return new JwtSecurityToken(
                "AnatechYazilim",
                "AnatechYazilim",
                SetJwtClaims(kullanici, roles),
                DateTime.Now,
                _accessTokenExpiration,
                signingCredentials);
        }

        private static IEnumerable<Claim> SetJwtClaims(Kullanici kullanici, IEnumerable<Role> roles)
        {
            var claims = new List<Claim>();
            claims.AddRole(roles.Select(p => p.Name).AsEnumerable());
            return claims;
        }

    }
}
