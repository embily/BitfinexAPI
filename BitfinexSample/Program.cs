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

            // Environment Varible BitfinexApi_key, you got to set it up --
            string key = Configuration["BitfinexApi_key"];

            // Environment Varible BitfinexApi_secret, you got to set it up --
            string secret = Configuration["BitfinexApi_secret"];

            BitfinexApiV1 api = new BitfinexApiV1(key, secret);

            // Balances - original way --
            //BalancesResponse bal = api.GetBalances();
            //System.Console.WriteLine($"Bal {bal.totalAvailableBTC}");

            // Modernized way --
            {
                // Account Info(s) 
                var response = api.AccountInfosAsync().Result;
                Console.WriteLine($"Account Info: {response}");
                Console.WriteLine($"Account Info: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
            }
            {
                // Summary 
                var response = api.SummaryAsync().Result;
                Console.WriteLine($"Summary: {response}");
                Console.WriteLine($"Summary: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
            }

        }

        static void Configure()
        {
            // Bitfinex API key and secret are stored in enviroment variables, 
            // thus, we need access to Environment Varibles to get them --   
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}
