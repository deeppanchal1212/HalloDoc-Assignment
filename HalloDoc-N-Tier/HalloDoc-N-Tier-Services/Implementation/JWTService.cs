using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using HalloDoc_N_Tier_Repository.Interface;
using HalloDoc_N_Tier_Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Services.Implementation
{
    public class JWTService : IJWTService
    {
        private IConfiguration _config;
        private IPatientRepo _repo;

        public JWTService (IConfiguration config,IPatientRepo repo)
        {
            _config = config;
            _repo = repo;
        }
        
        public string GenerateToken(GenetareTokenViewModel userdata)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userdata.Email),
                new Claim(ClaimTypes.Role,userdata.Role)
            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(20);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claim,
                expires: expires,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTokenForEmail(int reqid)
        {
            var claim = new List<Claim>
            {
                new Claim ("RequestId", reqid.ToString())
            };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claim,
                expires: expires,
                signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Request ValidateTokenForId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "");
            var tokenvalidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
       ,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Adjust the clock skew if needed
            };
            var principal = tokenHandler.ValidateToken(token, tokenvalidationParameters, out var validatedToken);
            var reqid = principal.FindFirst("RequestId")?.Value;
            int requestid = 0;
            if (reqid != null)
            {
                requestid = int.Parse(reqid);
            }

            Request req = _repo.GetRequest(requestid);
            if (req != null)
            {
                return req;
            }
            else
            {
                return new();
            }
        }

        public bool ValidateToken(string token, out JwtSecurityToken validatedToken)
        {
            validatedToken = null;

            if (token == null)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)

,
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Adjust the clock skew if needed
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                validatedToken = securityToken as JwtSecurityToken;

                // Optionally, you can access the claims in the principal:
                // var userId = principal.FindFirst("UserId")?.Value;
                // var userName = principal.FindFirst("UserName")?.Value;

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
