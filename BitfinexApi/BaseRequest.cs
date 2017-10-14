using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitfinexApi
{
    public class BaseRequest
    {
        [JsonProperty("request")]
        internal string Request { get; set; }

        [JsonProperty("nonce")]
        internal string Nonce
        {
            get
            {
                // not an ideal fix, why should be 1,000 multiplied (https://github.com/bitfinexcom/bitfinex-api-node/issues/111), kind of ... -- 
                return (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000).ToString();
            }
            private set { }
        }
    }
}
