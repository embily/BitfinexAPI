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
                throw new BitfinexException(r.Result);
            }
            return r;
        }

        public async Task<HistoryResponse[]> HistoryAsync(HistoryRequest request)
        {
            request.Request = "/v1/history/movements";

            var r = await SendRequestAAsync<HistoryResponse>(request);
            return r;
        }

        public async Task<NewOrderResponse> NewOrderAsync(NewOrderRequest request)
        {
            request.Request = "/v1/order/new";

            var r = await SendRequestOAsync<NewOrderResponse>(request);
            return r;
        }

        public async Task<OrderStatusResponse> OrderStatusAsync(OrderStatusRequest request)
        {
            request.Request = "/v1/order/status";

            var r = await SendRequestOAsync<OrderStatusResponse>(request);
            return r;
        }

        public async Task<PastTradesResponse[]> PastTradesAsync(PastTradesRequest request)
        {
            request.Request = "/v1/mytrades";

            var r = await SendRequestAAsync<PastTradesResponse>(request);
            return r;
        }

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
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Body Raw:");
                Console.WriteLine($"{body}");

                if(!response.IsSuccessStatusCode)
                {
                    throw new BitfinexException(response.ReasonPhrase, body);
                }

                return body;
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
