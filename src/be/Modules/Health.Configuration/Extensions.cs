using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Configuration
{
    /// <summary>
    /// Holds a set of handy helpers
    /// </summary>
    public static class Extensions
    {
        private static readonly Random random = new Random();
        
        /// <summary>
        /// Bind a configuration section to a specific as singleton
        /// </summary>
        /// <typeparam name="T">The targeted generic type</typeparam>
        /// <param name="services">The service collection to attach the singleton</param>
        /// <param name="section">The configuration section to bind into it</param>
        public static void ConfigureSection<T>(this IServiceCollection services, IConfiguration section)
        {
            var configInstance = Activator.CreateInstance<T>();

            section.Bind(configInstance);

            services.AddSingleton(typeof(T), configInstance);
        }

        /// <summary>
        /// Retrieves a random number string
        /// </summary>
        public static string GetRandomNumber()
        {
            return random.Next(54321, 99999).ToString();
        }
    }
}
