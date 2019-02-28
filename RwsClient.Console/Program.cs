using System;
using System.Threading.Tasks;
using Medidata.RWS.NET.Standard.Core;
using Medidata.RWS.NET.Standard.Core.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Sinks.File;
using Serilog.Extensions.Logging;

namespace RwsClient.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("consoleapp.log")
                .CreateLogger();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var myClass = serviceProvider.GetService<MyClass>();
            myClass.SomeMethod().GetAwaiter().GetResult();

            //MainAsync(args).GetAwaiter().GetResult();

            System.Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog())
                    //.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                    .AddTransient<MyClass>();

        }

        public static async Task MainAsync(string[] args)
        {
            try
            {
                var connection = new RwsConnection("tri");
                var response = await connection.SendRequestAsync(new FakeRwsRequest()) as RwsTextResponse;
                
                if (response != null)
                    System.Console.Write(await response.ResponseObject.Content.ReadAsStringAsync());
                else
                    System.Console.Write("No Response...");
            }
            catch(Exception ex)
            {
                System.Console.Write("Something Went Wrong!: " + ex);
            }
            
        }
    }
}
