using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using BitfinexApi;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BitfinexSample
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configure();

            // **** API Samples ***** //
            // uncomment the desired one! 

            //AccountInfosSample().Wait();

            //SummarySample().Wait();

            //DepositSample().Wait();

            //HistorySample().Wait();

            NewOrderAndOrderStatusSample().Wait();
        }

        static void Configure()
        {
            // Bitfinex API key and secret are stored in enviroment variables, 
            // thus, we need access to Environment Varibles to get them --   
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        static async Task AccountInfosSample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);

            var response = await api.AccountInfosAsync();

            LogResponse(response);
        }

        static async Task SummarySample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);

            var response = await api.SummaryAsync();

            LogResponse(response);
        }

        static async Task DepositSample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);
            
            var request = new DepositRequest
            {
                Method = "bitcoin",
                WalletName = "exchange",
                Renew = 1,
            };

            LogRequest(request);

            var response = await api.DepositAsync(request);

            LogResponse(response);
        }

        static async Task HistorySample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);

            var request = new HistoryRequest
            {
                Currency = "BTC",
                Method = "bitcoin",
                Since = DateTimeOffset.Now.AddDays(-30).ToUnixTimeSeconds().ToString(),
                Until = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds().ToString(),
                Limit = 100,
            };

            LogRequest(request);

            var response = await api.HistoryAsync(request);

            LogResponse(response);
        }

        static async Task NewOrderAndOrderStatusSample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);

            var request = new NewOrderRequest
            {
                Symbol = OrderSymbols.BTCUSD,
                Amount = 0.005.ToString(),
                // Price to buy or sell at. Must be positive. Use random number for market orders.
                Price = new Random().NextDouble().ToString(),
                Side = OrderSides.Sell,
                Type = OrderTypes.ExchangeMarket,
            };

            LogRequest(request);

            var response = await api.NewOrderAsync(request);

            LogResponse(response);

            long orderId = response.OrderId;

            Retry.Do(() => { OrderStatusSample(orderId).Wait(); }, TimeSpan.FromSeconds(3));
        }

        static async Task OrderStatusSample(long orderId)
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);

            var request = new OrderStatusRequest
            {
                OrderId = orderId
            };

            LogRequest(request);

            var response = await api.OrderStatusAsync(request);

            LogResponse(response);

            if(response.IsLive)
            {
                throw new ApplicationException("Order is still being executed. Waiting for completion.");
            }

        }

        private static void LogRequest(BaseRequest request)
        {
            Console.WriteLine($"Request: {request}");
            Console.WriteLine($"Request: {JsonConvert.SerializeObject(request, Formatting.Indented)}");
        }

        private static void LogResponse(object response)
        {
            Console.WriteLine($"Response: {response}");
            Console.WriteLine($"Respose: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }
    }
}
