using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitfinexApi
{
    public class HistoryRequest : BaseRequest
    {
        /// <summary>
        /// The currency to look for
        /// </summary>
        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }

        /// <summary>
        /// The method of the deposit/withdrawal (can be “bitcoin”, “litecoin”, “darkcoin”, “wire”).
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Return only the history after this timestamp.
        /// </summary>
        [JsonProperty("since")]
        public string Since { get; set; }

        /// <summary>
        /// Return only the history before this timestamp.
        /// </summary>
        [JsonProperty("until")]
        public string Until { get; set; }

        /// <summary>
        /// Limit the number of entries to return.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}
