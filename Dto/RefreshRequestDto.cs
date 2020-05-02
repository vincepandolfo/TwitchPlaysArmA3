using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchPlaysArmA3.Dto
{
    class RefreshRequestDto
    {
        [JsonProperty("client_id")]
        public string clientId;
        [JsonProperty("client_secret")]
        public string clientSecret;
        [JsonProperty("refresh_token")]
        public string refreshToken;
        [JsonProperty("grant_type")]
        public string grantType = "refresh_token";
    }
}
