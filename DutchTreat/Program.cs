using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            // Instantiate and buildup Seeder
            RunSeeding(host);
            host.Run();
        }

        private static void RunSeeding(IWebHost host)
        {

            //Before we use Seed(), we need to fulfill all the dependencies
            //Go to Startup.cs and ad the services for same
            //Even though we know that it is a scoped dependency we'll still verify if it's a scoped dependency or not.
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            //This is the way outside of the web server to create a scope. 
            //Interesting thing about it is that during every request the scopeFactory actually creates the scope for entire lifetime of the request.
            //I.e. we get the instance of the context object that is true throught the entire request.
            using (var scope = scopeFactory.CreateScope())
            {
                //var seeder = host.Services.GetService<DutchSeeder>();//Microsoft.Extensions.DependencyInjection
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(SetupConfiguration)
                   .UseStartup<Startup>();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Removing the default configuration options.
            builder.Sources.Clear();
            builder.AddJsonFile("config.json", false, true)
                   .AddEnvironmentVariables();
            //This will automatically reload the changes done in config files unlike previously.
            //The heirarchy at which it will implement the config is bottom to top i.e. AddEnvironmentVariables will override the config changes above it.
            //AddEnvironmentVariables - usually done by IT team in production deployments
        }
    }
}
