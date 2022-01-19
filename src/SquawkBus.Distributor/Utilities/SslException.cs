using System;
using System.Net;

namespace SquawkBus.Distributor.Utilities
{
    public class SslException : Exception
    {
        public SslException(IPAddress address)
            : base()
        {
            Address = address;
        }

        public IPAddress Address { get; }
    }
}