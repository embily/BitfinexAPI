using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace BitfinexApi
{
    public class BitfinexException : WebException
    {

        public BitfinexException(WebException ex, string bitfinexMessage) : base(bitfinexMessage, ex)
        {
        }

        public BitfinexException(string reason, string bitfinexMessage) : base($"Resason: [{reason}]; Bitfinex message: [{bitfinexMessage}]")
        {
        }

        public BitfinexException(string bitfinexMessage) : base($"Bitfinex message: [{bitfinexMessage}]")
        {
        }
    }
}
