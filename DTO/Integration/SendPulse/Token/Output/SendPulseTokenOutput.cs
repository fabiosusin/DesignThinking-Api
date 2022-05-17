﻿using Newtonsoft.Json;

namespace DTO.Integration.SendPulse.Token.Output
{
    public class SendPulseTokenOutput
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
