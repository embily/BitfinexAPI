using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitfinexApi
{
    public class DepositRequest : BaseRequest
    {
        /// <summary>
        /// Method of deposit (methods accepted: “bitcoin”, “litecoin”, “ethereum”, “tetheruso", "ethereumc", "zcash", "monero", "iota", "bcash").
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Wallet to deposit in (accepted: “trading”, “exchange”, “deposit”). Your wallet needs to already exist
        /// </summary>
        [JsonProperty("wallet_name")]
        public string WalletName { get; set; }

        /// <summary>
        /// Default is 0. If set to 1, will return a new unused deposit address
        /// </summary>
        [JsonProperty("renew")]
        public int Renew { get; set; }
    }
}
