using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Identity.JWT
{
    public class JwtOptions
    {
        [JsonPropertyName("secretkey")]
        public string SecretKey { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("accessTokenExpiration")]
        public int AccessTokenExpiration { get; set; }
    }
}
