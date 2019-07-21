using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace accountapi
{
    public class Program
    {
        public static void Main( string[] args )
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {

            var config = GetServerUrlsFromCommandLine(args);
            var port = config.GetValue<int>("port");
            var hostUrl = config.GetValue<string>("server.urls");

            var builder = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(hostUrl)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    //logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                });
            
            return builder;
        }

        public static IConfigurationRoot GetServerUrlsFromCommandLine(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var serverport = config.GetValue<int?>("port") ?? 80;
            var serverurls = config.GetValue<string>("server.urls") ?? string.Format("http://0.0.0.0:{0}", serverport);

            var configDictionary = new Dictionary<string, string>
            {
                {"server.urls", serverurls},
                {"port", serverport.ToString()}
            };

            return new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddInMemoryCollection(configDictionary)
                .Build();
        }

    }
}
