using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using SportsEventApp.Infrastructure.DataAccess.Repository;
using SportsEventApp.Infrastructure.DataAccess.Repository.Base;
using System.Net;

namespace SportsEventApp.Silo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(config =>
                    {
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<ISportEventRepository, SportEventRepository>();
                        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
                    })
                    .UseOrleans(siloBuilder =>
                    {
                        siloBuilder
                            .UseLocalhostClustering()
                            .AddMemoryGrainStorage("SportEventStore") // burası önemli
                            .Configure<ClusterOptions>(options =>
                            {
                                options.ClusterId = "dev";
                                options.ServiceId = "SportEventApp";
                            });
                    })
                    .ConfigureLogging(logging => logging.AddConsole())
                    .Build();

            await host.RunAsync();
        }
    }
}
