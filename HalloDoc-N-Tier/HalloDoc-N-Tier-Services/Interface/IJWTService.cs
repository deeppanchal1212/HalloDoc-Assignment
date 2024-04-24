using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Services.Interface
{
    public interface IJWTService
    {
        string GenerateToken(GenetareTokenViewModel userdata);

        public bool ValidateToken(string token, out JwtSecurityToken validatedToken);

        public string GenerateTokenForEmail(int reqid);

        public Request ValidateTokenForId(string token);

    }
}
