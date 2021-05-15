using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quiz.App.Entities;

namespace Quiz.App.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private IConfiguration Configuration;
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;


        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }


        public AccessToken CreateToken(User user)
        {
            _accessTokenExpiration = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwtToken = CreateJwtSecurityToken(user, signingCredentials);


            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtToken);

            return new AccessToken  //KULLANICIYA DÖNÜLECEK OLAN NESNE
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                Name = $"{user.Name} {user.LastName}",
                UserId = user.Id,
                Email = user.Email
            };
        }


        public JwtSecurityToken CreateJwtSecurityToken(User user, SigningCredentials signingCredentials)

        {
            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: new[]
                {
                    new Claim("userId",user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("name", $"{user.Name} {user.LastName}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                },

                signingCredentials: signingCredentials
            );

            return jwt;
        }


    }
}