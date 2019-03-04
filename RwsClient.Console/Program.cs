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
            AppDomain.CurrentDomain.UnhandledException += Current_DomainUnHandledException;

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var myClass = serviceProvider.GetService<MyClass>();
            //var logger = serviceProvider.GetService<ILogger<Program>>();  

            using (var logger = BuildSerilog())
            {
                try
                {
                    myClass.SomeMethod().GetAwaiter().GetResult();
                }
                catch(Exception ex)
                {
                    logger.Fatal(ex, "Console crashed");
                }
            }
            

            //MainAsync(args).GetAwaiter().GetResult();

            System.Console.Write(Environment.NewLine + "Execution complete...");
            System.Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog())
                    //.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                    .AddTransient<MyClass>();

        }

        protected static void Current_DomainUnHandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            if (Log.Logger != null && e.ExceptionObject is Exception exception)
            {
                UnHandledExceptions(exception);

                if (e.IsTerminating)
                {
                    Log.CloseAndFlush();
                }
            }
        }

        private static void UnHandledExceptions(Exception e)
        {
            Log.Logger?.Error(e, "Console application crashed");
        }

        private static Logger BuildSerilog()
        {
            var logger = new LoggerConfiguration()
                            .WriteTo.File("consoleapp.log")
                            .CreateLogger();

            Log.Logger = logger;

            return logger;
        }

        public static async Task MainAsync(string[] args)
        {
            try
            {
                var connection = new RwsConnection("tri");
                var response = await connection.SendRequestAsync(new MyTwoHundreadRequest()) as RwsTextResponse;
                
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
