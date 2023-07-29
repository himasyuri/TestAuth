using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthCommon
{
    public class AuthOptions
    {
        public string? Secret { get; set; }

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public int AccessTokenLifeTime { get; set; }

        public int RefreshTokenLifeTime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}