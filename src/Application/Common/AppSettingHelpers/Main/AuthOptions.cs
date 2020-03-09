using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Common.AppSettingHelpers.Main
{
    public class AuthOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public int ExpiryTimeTokenMinutes { get; set; }

        public int ExpiryTimeRefreshTokenMinutes { get; set; }

        public bool RequireHttpsMetadata { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SecretKey))
                {
                    throw new NullReferenceException(nameof(SecretKey));
                }

                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            }
        }
    }
}