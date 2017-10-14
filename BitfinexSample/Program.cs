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

            //AccountInfosSample().Wait();

            //SummarySample().Wait();

            //DepositSample().Wait();

            HistorySample().Wait();
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

            Console.WriteLine($"Account Info: {response}");
            Console.WriteLine($"Account Info: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }

        static async Task SummarySample()
        {
            var api = new BitfinexApiV1(Configuration["BitfinexApi_key"], Configuration["BitfinexApi_secret"]);
            
            var response = await api.SummaryAsync();

            Console.WriteLine($"Summary: {response}");
            Console.WriteLine($"Summary: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
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

            var response = await api.DepositAsync(request);

            Console.WriteLine($"Deposit: {response}");
            Console.WriteLine($"Deposit: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
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

            var response = await api.HistoryAsync(request);

            Console.WriteLine($"Deposit: {response}");
            Console.WriteLine($"Deposit: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }
    }
}
