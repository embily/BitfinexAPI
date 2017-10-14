using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitfinexApi
{
    public class BaseRequest
    {
        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}
