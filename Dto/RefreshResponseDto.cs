using Newtonsoft.Json;

namespace TwitchPlaysArmA3.Dto
{
    class RefreshResponseDto
    {
        [JsonProperty("access_token")]
        public string accessToken;
        [JsonProperty("refresh_token")]
        public string refreshToken;
    }
}
