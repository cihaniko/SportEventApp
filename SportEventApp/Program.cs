
using SportsEventApp.Infrastructure.DataAccess.Repository.Base;
using SportsEventApp.Infrastructure.DataAccess.Repository;
using Orleans.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using SportEventApp.HealthChecks;

namespace SportEventApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Host.UseOrleansClient(client =>
            {
                client.UseLocalhostClustering(); // local silo baðlantýsý

                client.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "SportEventApp";
                });
            });


            builder.Services.AddHealthChecks()
            .AddSqlServer(
                connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                name: "mssql",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "sql" }
            )
            .AddCheck<OrleansSelfCheck>("orleans");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<ISportEventRepository, SportEventRepository>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await initializer.InitializeAsync();
            }



            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        })
                    });
                    await context.Response.WriteAsync(result);
                }
            });

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
