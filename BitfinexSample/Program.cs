using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using BitfinexApi;
using Newtonsoft.Json;

namespace BitfinexSample
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configure();

            string key = Configuration["BitfinexApi_key"];
            string secret = Configuration["BitfinexApi_secret"];

            BitfinexApiV1 api = new BitfinexApiV1(key, secret);

            // Balances - original way --
            BalancesResponse bal = api.GetBalances();
            System.Console.WriteLine($"Bal {bal.totalAvailableBTC}");

            // Account Info(s) - modern way --  
            var response = api.AccountInfosAsync().Result;
            Console.WriteLine($"Account Info: {JsonConvert.SerializeObject(response, Formatting.Indented )}");
        }

        static void Configure()
        {

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}
