using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Text;

namespace BitfinexApi
{
    public class OrderStatusRequest : BaseRequest
    {
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
    }
}
