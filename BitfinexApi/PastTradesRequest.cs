using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Text;

namespace BitfinexApi
{
    public class PastTradesRequest : BaseRequest
    {
        /// <summary>
        /// The pair traded, can be “btcusd”, etc.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// REQUIRED
        /// Trades made before this timestamp won’t be returned.
        /// datetime, Unix seconds 
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Trades made after this timestamp won’t be returned.
        /// </summary>
        [JsonProperty("until")]
        public string Until { get; set; }

        /// <summary>
        /// Limit the number of trades returned.
        /// </summary>
        [JsonProperty("limit_trades")]
        public int LimitTrades { get; set; }

        /// <summary>
        /// Limit the number of entries to return.
        /// </summary>
        [JsonProperty("reverse")]
        public int Reverse { get; set; }
    }
}
