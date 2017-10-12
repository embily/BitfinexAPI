using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using BitfinexApi;

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

            BalancesResponse bal = api.GetBalances();

            System.Console.WriteLine($"Bal {bal.totalAvailableBTC}");
        }

        static void Configure()
        {

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}
