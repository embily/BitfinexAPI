using System;
using System.Collections.Generic;

using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BitfinexApi
{
    public class BitfinexApiV1
    {
        private const string _endpointAddress = "https://api.bitfinex.com";

        private HMACSHA384 _hashMaker;
        private string _key;

        public string Nonce
        {
            get
            {
                // not an ideal fix, why should be 1,000 multiplied (https://github.com/bitfinexcom/bitfinex-api-node/issues/111), kind of ... -- 
                return (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000).ToString();
            }
        }

        public BitfinexApiV1(string key, string secret)
        {
            _hashMaker = new HMACSHA384(Encoding.UTF8.GetBytes(secret));
            _key = key;
        }

        public async Task<AccountInfoResponse[]> AccountInfosAsync()
        {
            var request = new AccountInfosRequest
            {
                Request = "/v1/account_infos",
            };

            return await SendRequestAAsync<AccountInfoResponse>(request);
        }

        public async Task<SummaryResponse> SummaryAsync()
        {
            var request = new SummaryRequest
            {
                Request = "/v1/summary",
            };

            return await SendRequestOAsync<SummaryResponse>(request);
        }

        public async Task<DepositResponse> DepositAsync(DepositRequest request)
        {
            request.Request = "/v1/deposit/new";

            var r = await SendRequestOAsync<DepositResponse>(request);
            if(r.Result != "success")
            {
                throw new BitfinexException(null, r.Result);
            }
            return r;
        }

        public BalancesResponse GetBalances()
        {
            BalancesRequest req = new BalancesRequest(Nonce);
            string response = SendRequest(req, "GET"); // is it get but it works... --
            BalancesResponse resp = BalancesResponse.FromJSON(response);

            return resp;
        }

        public CancelOrderResponse CancelOrder(int order_id)
        {
            CancelOrderRequest req = new CancelOrderRequest(Nonce, order_id);
            string response = SendRequest(req, "POST");
            CancelOrderResponse resp = CancelOrderResponse.FromJSON(response);
            return resp;
        }

        public CancelAllOrdersResponse CancelAllOrders()
        {
            CancelAllOrdersRequest req = new CancelAllOrdersRequest(Nonce);
            string response = SendRequest(req, "GET"); // is it get??? --
            return new CancelAllOrdersResponse(response);
        }

        public OrderStatusResponse GetOrderStatus(int order_id)
        {
            OrderStatusRequest req = new OrderStatusRequest(Nonce, order_id);
            string response = SendRequest(req, "POST");
            return OrderStatusResponse.FromJSON(response);
        }

        public ActiveOrdersResponse GetActiveOrders()
        {
            ActiveOrdersRequest req = new ActiveOrdersRequest(Nonce);
            string response = SendRequest(req, "POST");
            return ActiveOrdersResponse.FromJSON(response);
        }

        public ActivePositionsResponse GetActivePositions()
        {
            ActivePositionsRequest req = new ActivePositionsRequest(Nonce);
            string response = SendRequest(req, "POST");
            return ActivePositionsResponse.FromJSON(response);
        }

        public NewOrderResponse ExecuteBuyOrderBTC(decimal amount, decimal price, OrderExchange exchange, OrderType type)
        {
            return ExecuteOrder(OrderSymbol.BTCUSD, amount, price, exchange, OrderSide.Buy, type);
        }

        public NewOrderResponse ExecuteSellOrderBTC(decimal amount, decimal price, OrderExchange exchange, OrderType type)
        {
            return ExecuteOrder(OrderSymbol.BTCUSD, amount, price, exchange, OrderSide.Sell, type);
        }

        public NewOrderResponse ExecuteOrder(OrderSymbol symbol, decimal amount, decimal price, OrderExchange exchange, OrderSide side, OrderType type)
        {
            NewOrderRequest req = new NewOrderRequest(Nonce, symbol, amount, price, exchange, side, type);
            string response = SendRequest(req, "POST");
            NewOrderResponse resp = NewOrderResponse.FromJSON(response);
            return resp;
        }

        private string SendRequest(GenericRequest request, string httpMethod)
        {
            string json = JsonConvert.SerializeObject(request);
            string json64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            byte[] data = Encoding.UTF8.GetBytes(json64);
            byte[] hash = _hashMaker.ComputeHash(data);
            string signature = GetHexString(hash);

            HttpWebRequest wr = WebRequest.Create("https://api.bitfinex.com" + request.request) as HttpWebRequest;
            wr.Headers.Add("X-BFX-APIKEY", _key);
            wr.Headers.Add("X-BFX-PAYLOAD", json64);
            wr.Headers.Add("X-BFX-SIGNATURE", signature);
            wr.Method = httpMethod;

            string response = null;
            try
            {
                HttpWebResponse resp = wr.GetResponse() as HttpWebResponse;
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                response = sr.ReadToEnd();
                sr.Close();
            }
            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                response = sr.ReadToEnd();
                sr.Close();
                throw new BitfinexException(ex, response);
            }
            return response;
        }

        // Modernized http calls with HttpClient, Generics, and Asyncs. I think it is a bit cooler this way -- 

        private async Task<T> SendRequestOAsync<T>(BaseRequest request)
        {
            var responseBody = await SendRequestAsync(request, request.Request);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private async Task<T[]> SendRequestAAsync<T>(BaseRequest request)
        {
            var responseBody = await SendRequestAsync(request, request.Request);
            return JsonConvert.DeserializeObject<T[]>(responseBody);
        }

        private async Task<string> SendRequestAsync(object request, string url)
        {
            string json = JsonConvert.SerializeObject(request);
            string json64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            byte[] data = Encoding.UTF8.GetBytes(json64);
            byte[] hash = _hashMaker.ComputeHash(data);
            string signature = GetHexString(hash);

            using (HttpClient client = new HttpClient())
            {
                var headers = client.DefaultRequestHeaders;
                headers.Add("X-BFX-APIKEY", _key);
                headers.Add("X-BFX-PAYLOAD", json64);
                headers.Add("X-BFX-SIGNATURE", signature);

                var response = await client.PostAsync(_endpointAddress + url, null);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        private String GetHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(String.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
    }
}
