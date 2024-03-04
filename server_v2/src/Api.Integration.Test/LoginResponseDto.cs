using Newtonsoft.Json;

namespace Api.Integration.Test
{
    public class LoginResponseDto
    {
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }

        [JsonProperty("create")]
        public DateTimeOffset CreateAt { get; set; }

        [JsonProperty("expiration")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
