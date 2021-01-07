using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using System;
using System.IO;

namespace StockExchange
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseContentRoot(Directory.GetCurrentDirectory())
              .ConfigureServices(services => services.AddAutofac())
              .ConfigureAppConfiguration((hostContext, config) =>
              {
                  var env = hostContext.HostingEnvironment;
                  Console.WriteLine("Env:" + env.EnvironmentName);
                  config.SetBasePath(Path.Combine(env.ContentRootPath, "Configuration"))
                        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
              })
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();
    }
}
