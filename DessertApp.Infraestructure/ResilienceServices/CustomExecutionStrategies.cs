using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace DessertApp.Infraestructure.ResilienceServices
{
    internal class CustomExecutionStrategies : ExecutionStrategy
    {
        public CustomExecutionStrategies(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) : base(context, maxRetryCount, maxRetryDelay)
        {
        }

        public CustomExecutionStrategies(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay) : base(dependencies, maxRetryCount, maxRetryDelay)
        {
            
        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            if (exception is SqlException sqlException)
            {
                var transientErrors = new[] { -2, 4064,40197, 40501, 40613 };
                var criticalErrors = new[] { 4060, 18456 };

                if (transientErrors.Contains(sqlException.Number))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Retryable error detected: {sqlException.Message}");
                    Console.WriteLine($"Current retry attempt: {ExceptionsEncountered.Count} of {MaxRetryCount}");
                    return true;
                }
                if (criticalErrors.Contains(sqlException.Number))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Critical database error: {sqlException.Message}");
                    return false;

                }

                if (ExceptionsEncountered.Count == MaxRetryCount)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Sending a critical email...");
                }
            }
            return false;
        }

        protected override TimeSpan? GetNextDelay(Exception lastException)
        {
            var delay = base.GetNextDelay(lastException);
            if (delay.HasValue)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Retrying after {delay.Value.TotalSeconds} seconds due to: {lastException.Message}");
                Console.WriteLine($"Current retry attempt: {ExceptionsEncountered.Count}");
            }
            return delay;
        }
    }
}
