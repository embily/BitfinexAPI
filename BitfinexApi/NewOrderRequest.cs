using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace BitfinexApi
{
    /// <summary>
    /// The name of the symbol (see /symbols).
    /// </summary>
    public enum OrderSymbols
    {
        [EnumMember(Value = "btcusd")]
        BTCUSD
    }

    /// <summary>
    /// type starting by "exchange " are exchange orders, others are margin trading orders
    /// </summary>
    public enum OrderTypes
    {
        [EnumMember(Value = "market")]
        Market,

        [EnumMember(Value = "limit")]
        Limit,

        [EnumMember(Value = "stop")]
        Stop,

        [EnumMember(Value = "trailing-stop")]
        TrailingStop,

        [EnumMember(Value = "fill-or-kill")]
        FillOrKill,

        [EnumMember(Value = "exchange market")]
        ExchangeMarket,

        [EnumMember(Value = "exchange limit")]
        ExchangeLimit,

        [EnumMember(Value = "exchange stop")]
        ExchangeStop,

        [EnumMember(Value = "exchange trailing-stop")]
        ExchangeTrailingStop,

        [EnumMember(Value = "exchange fill-or-kill")]
        ExchangeFillOrKill
    }

    public enum OrderSides
    {
        [EnumMember(Value = "buy")]
        Buy,

        [EnumMember(Value = "sell")]
        Sell
    }

    public enum OrderExchanges
    {
        [EnumMember(Value = "bitfinex")]
        Bitfinex,

        [EnumMember(Value = "bitstamp")]
        Bitstamp,

        [EnumMember(Value = "all")]
        All
    }

    public class NewOrderRequest : BaseRequest
    {
        /// <summary>
        /// The name of the symbol (see /symbols).
        /// see: https://docs.bitfinex.com/v1/reference#rest-public-symbols
        /// </summary>
        [JsonRequired]
        [JsonProperty("symbol")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSymbols Symbol { get; set; }

        /// <summary>
        /// Order size: how much you want to buy or sell
        /// </summary>
        [JsonRequired]
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Price to buy or sell at. Must be positive. Use random number for market orders.
        /// </summary>
        [JsonRequired]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Either “buy” or “sell”.
        /// </summary>
        [JsonRequired]
        [JsonProperty("side")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSides Side { get; set; }

        /// <summary>
        /// Either “market” / “limit” / “stop” / “trailing-stop” / “fill-or-kill” / “exchange market” / 
        /// “exchange limit” / “exchange stop” / “exchange trailing-stop” / “exchange fill-or-kill”. 
        /// (type starting by “exchange ” are exchange orders, others are margin trading orders)
        /// </summary>
        [JsonRequired]
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderTypes Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("exchange")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderExchanges Exchange { get; set; }

        /// <summary>
        /// true if the order should be hidden.
        /// </summary>
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        /// <summary>
        /// true if the order should be post only. Only relevant for limit orders.
        /// </summary>
        [JsonProperty("is_postonly")]
        public bool IsPostonly { get; set; }

        /// <summary>
        /// 1 will post an order that will use all of your available balance.
        /// </summary>
        [JsonProperty("use_all_available")]
        public int UseAllAvailable { get; set; }

        /// <summary>
        /// Set an additional STOP OCO order that will be linked with the current order.
        /// </summary>
        [JsonRequired]
        [JsonProperty("ocoorder")]
        public bool Ocoorder { get; set; }

        /// <summary>
        /// If ocoorder is true, this field represent the price of the OCO stop order to place
        /// </summary>
        [JsonProperty("buy_price_oco")]
        public string BuyPriceOco { get; set; }

        /// <summary>
        /// If ocoorder is true, this field represent the price of the OCO stop order to place
        /// </summary>
        //[JsonRequired]
        //[DefaultValue(null)]
        //[JsonProperty("sell_price_oco", DefaultValueHandling = DefaultValueHandling.Populate)]
        [JsonProperty("sell_price_oco")]
        public string SellPriceOco { get; set; }
    }
}
