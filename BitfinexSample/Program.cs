using System;
using BitfinexApi;

namespace BitfinexSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "your_key"; string secret = "your_secret";

            BitfinexApiV1 api = new BitfinexApiV1(key, secret);

            BalancesResponse bal = api.GetBalances();
        }
    }
}
