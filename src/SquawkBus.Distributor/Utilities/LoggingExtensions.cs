using Microsoft.Extensions.Logging;

using SquawkBus.Distributor.Configuration;

namespace SquawkBus.Distributor.Utilities
{
    public static class LoggingExtensions
    {
        public static ILoggingBuilder AddDistributorLogger(this ILoggingBuilder builder, DistributorConfig config)
        {
            return config.UseJsonLogger ? builder.AddJsonConsole() : builder.AddConsole();
        }
    }
}